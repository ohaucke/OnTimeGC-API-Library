using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using OnTimeGC_API.Exception;
using OnTimeGC_API.Objects;

namespace OnTimeGC_API
{
    /// <summary>
    /// OnTimeGC API Client
    /// </summary>
    public class Client
    {
        private Uri onTineGCUrl;
        private Main basis;
        private CookieContainer cc;

        #region constructor
        /// <summary>
        /// Initiate the client
        /// </summary>
        /// <param name="onTimeGCUrl">API Endpoint (eg. http://demo.ontimesuite.com/ontime/ontimegcweb.nsf )</param>
        /// <param name="apiVersion">The version of the API you are using</param>
        /// <param name="applicationid">Application ID - must be present in the license key</param>
        /// <param name="applicationVersion">The application version number</param>
        public Client(string onTimeGCUrl, int apiVersion, string applicationid, string applicationVersion)
        {
#if DEBUG
            System.Net.WebRequest.DefaultWebProxy = new WebProxy("localhost", 8880);
#endif

            this.onTineGCUrl = new Uri(onTimeGCUrl);
            this.cc = new CookieContainer();

            this.basis = new Main()
            {
                APIVersion = apiVersion,
                ApplicationId = applicationid,
                ApplicationVersion = applicationVersion
            };
        }
        #endregion

        #region Login
        /// <summary>
        /// Used to verify token
        /// </summary>
        /// <param name="token">OnTime Group Calendar token</param>
        /// <returns>Basic informations about the user</returns>
        public LoginItem Login(string token)
        {
            this.Token = token;

            return Login();
        }

        /// <summary>
        /// Use to get a valid token.
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <returns>Basic informations about the user</returns>
        public LoginItem Login(string username, string password)
        {
            TokenResult tr = InitialLogin(username, password);
            LoginItem lr = null;
            if (tr.Status.ToLower() == "ok")
            {
                lr = Login();
            }
            else
            {
                lr = new LoginItem();
                lr.Status = "Error";
                lr.Error = tr.Error;
            }

            return lr;
        }

        private LoginItem Login()
        {
            string pd = string.Format("{{{0},{1}}}", this.basis.ToString(), "\"Login\":{}");
            string s = Send(pd, EndPoint.ApiHttp);

            return s.ToObject<LoginItem>();
        }

        private TokenResult InitialLogin(string username, string password)
        {
            TokenResult result = null;
            if (GetLtpaCookie(username, password))
            {
                result = GetToken();
                if (result.Status.ToLower() == "ok")
                    this.basis.Token = result.Token;
            }
            else
            {
                result = new TokenResult();
                result.IsAnonymous = true;
                result.Status = "Error";
                //result.Error = "Username / password or token wrong (anonymous access)";
                result.Error = "Incorrect username, password or token (anonymous access)";
            }

            return result;
        }
        #endregion

        #region Logout
        /// <summary>
        /// Inform the api that we logout. Not critical, more as good behavior and logging
        /// </summary>
        /// <returns>Basic informations about the user</returns>
        /// <exception cref="OnTimeGC_API.Exception.InvalidTokenException"></exception>
        public LogoutItem Logout()
        {
            string pd = string.Format("{{{0},{1}}}", this.basis.ToString(), "\"Logout\":{}");
            string s = Send(pd, EndPoint.ApiHttp);
            CheckToken(s);

            return s.ToObject<LogoutItem>();
        }
        #endregion

        #region Version
        /// <summary>
        /// Some version information of API and running server
        /// </summary>
        /// <returns>some version information of API and running server</returns>
        /// <exception cref="OnTimeGC_API.Exception.InvalidTokenException"></exception>
        public OnTimeGC_API.Objects.Version Version()
        {
            string pd = string.Format("{{{0},{1}}}", this.basis.ToString(), "\"Version\":{}");
            string s = Send(pd, EndPoint.ApiHttp);
            CheckToken(s);

            return s.ToObject<VersionResult>().Item;
        }
        #endregion

        #region Calendars
        /// <summary>
        /// All calendar entries for specific users for a given time range
        /// </summary>
        /// <param name="ids">IDs for which it should return user info</param>
        /// <param name="startDate">Start date time</param>
        /// <param name="endDate">End date time</param>
        /// <returns>All calendar entries for specific users for a given time range</returns>
        /// <exception cref="OnTimeGC_API.Exception.InvalidTokenException"></exception>
        /// <exception cref="OnTimeGC_API.Exception.InvalidApiResponseException"></exception>
        public CalendarsItem Calendars(string[] ids, DateTime startDate, DateTime endDate)
        {
            string pd = string.Format("{{{0},{1}}}", this.basis.ToString(), "\"Calendars\":{\"IDs\":[\"" + string.Join("\",\"", ids) + "\"],\"FromDT\":" + startDate.ToOTDateTime() + ",\"ToDT\":" + endDate.ToOTDateTime() + "}");
            string s = Send(pd, EndPoint.ApiHttp);
            CheckToken(s);

            Regex pat = new Regex("\"Users\":(?<m>(.*)),\"UsersIDs\"");
            if (!pat.Match(s).Success || s.Contains("rs\":{},\"Us"))
                throw new InvalidApiResponseException("OnTimeGC API - Invalid API-Response", new System.Exception("API Result: " + s));

            string src = pat.Match(s).Groups["m"].Value;
            string dest = "[" + src.TrimStart('{').TrimEnd('}') + "}]";
            dest = Regex.Replace(dest, "\"[^\"]*\":{", "{");
            s = s.Replace(src, dest);

            return s.ToObject<CalendarsResult>().Item;
        }
        #endregion

        #region GroupList
        /// <summary>
        /// List of all available groups
        /// </summary>
        /// <returns>List of all available groups</returns>
        /// <exception cref="OnTimeGC_API.Exception.InvalidTokenException"></exception>
        /// <exception cref="OnTimeGC_API.Exception.InvalidApiResponseException"></exception>
        public List<GroupListItem> GroupList()
        {
            string pd = string.Format("{{{0},{1}}}", this.basis.ToString(), "\"GroupList\":{}");
            string s = Send(pd, EndPoint.ApiHttp);
            CheckToken(s);

            Regex pat = new Regex("\"GroupList\":(?<m>(.*)),\"Status\"");
            if (!pat.Match(s).Success)
                throw new InvalidApiResponseException("OnTimeGC API - Invalid API-Response", new System.Exception("API Result: " + s));

            string src = pat.Match(s).Groups["m"].Value;
            string dest = "[" + src.Replace("\"Items\":{", "").TrimStart('{').TrimEnd('}') + "}]";
            dest = Regex.Replace(dest, "\"[^\"]*\":{", "{");
            s = s.Replace(src, dest);

            return s.ToObject<GroupListResult>().Items;
        }
        #endregion

        #region LanguageList
        /// <summary>
        /// List of all available languages
        /// </summary>
        /// <returns>List of all available languages</returns>
        /// <exception cref="OnTimeGC_API.Exception.InvalidTokenException"></exception>
        /// <exception cref="OnTimeGC_API.Exception.InvalidApiResponseException"></exception>
        public List<LanguageListItem> LanguageList()
        {
            string pd = string.Format("{{{0},{1}}}", this.basis.ToString(), "\"LanguageList\":{}");
            string s = Send(pd, EndPoint.ApiHttp);
            CheckToken(s);

            Regex pat = new Regex("\"LanguageList\":(?<m>(.*)),\"Status\"");
            if (!pat.Match(s).Success)
                throw new InvalidApiResponseException("OnTimeGC API - Invalid API-Response", new System.Exception("API Result: " + s));
            string src = pat.Match(s).Groups["m"].Value;
            string dest = "[" + src.TrimStart('{').TrimEnd('}') + "}]";
            dest = Regex.Replace(dest, "\"[^\"]*\":{", "{");
            s = s.Replace(src, dest);

            return s.ToObject<LanguageListResult>().Items;
        }
        #endregion

        #region LanguageText
        /// <summary>
        /// All text in a given language
        /// </summary>
        /// <param name="languageCode">Language code for returned texts</param>
        /// <returns>All text in a given language - Returned format is "as is" in database.</returns>
        /// <exception cref="OnTimeGC_API.Exception.InvalidTokenException"></exception>
        public LanguageTextItem LanguageText(string languageCode)
        {
            string pd = string.Format("{{{0},{1}}}", this.basis.ToString(), "\"LanguageText\":{\"LanguageCode\":\"" + languageCode + "\"}");
            string s = Send(pd, EndPoint.ApiHttp);
            CheckToken(s);

            return s.ToObject<LanguageTextResult>().Item;
        }
        #endregion

        #region Legends
        /// <summary>
        /// All legends and texts in the given language
        /// </summary>
        /// <param name="languageCode">Language code for returned texts</param>
        /// <returns>All legends and texts in the given language</returns>
        /// <exception cref="OnTimeGC_API.Exception.InvalidTokenException"></exception>
        /// <exception cref="OnTimeGC_API.Exception.InvalidApiResponseException"></exception>
        public LegendsItem Legends(string languageCode)
        {
            string pd = string.Format("{{{0},{1}}}", this.basis.ToString(), "\"Legends\":{\"LanguageCode\":\"" + languageCode + "\"}");
            string s = Send(pd, EndPoint.ApiHttp);
            CheckToken(s);

            Regex pat = new Regex("\"LanguageCode\":\"(..|)\",\"Items\":(?<m>(.*))\\},\"Status\"");
            if (!pat.Match(s).Success)
                throw new InvalidApiResponseException("OnTimeGC API - Invalid API-Response", new System.Exception("API Result: " + s));

            string src = pat.Match(s).Groups["m"].Value;
            string dest = "[" + src.TrimStart('{').TrimEnd('}') + "}]";
            dest = Regex.Replace(dest, "\"[^\"]*\":{", "{");
            s = s.Replace(src, dest);

            return s.ToObject<LegendsResult>().Item;
        }
        #endregion

        #region RegionList
        /// <summary>
        /// List of all available Regions
        /// </summary>
        /// <returns>List of all available Regions</returns>
        /// <exception cref="OnTimeGC_API.Exception.InvalidTokenException"></exception>
        /// <exception cref="OnTimeGC_API.Exception.InvalidApiResponseException"></exception>
        public List<RegionListItem> RegionList()
        {
            string pd = string.Format("{{{0},{1}}}", this.basis.ToString(), "\"RegionList\":{}");
            string s = Send(pd, EndPoint.ApiHttp);
            CheckToken(s);

            Regex pat = new Regex("\"RegionList\":(?<m>(.*)),\"Status\"");
            if (!pat.Match(s).Success)
                throw new InvalidApiResponseException("OnTimeGC API - Invalid API-Response", new System.Exception("API Result: " + s));
            string src = pat.Match(s).Groups["m"].Value;
            string dest = "[" + src.TrimStart('{').TrimEnd('}') + "}]";
            dest = Regex.Replace(dest, "\"[^\"]*\":{", "{");
            s = s.Replace(src, dest);

            return s.ToObject<RegionListResult>().Items;
        }
        #endregion

        #region RegionText
        /// <summary>
        /// All region in a given language
        /// </summary>
        /// <param name="languageCode">Language code for returned texts</param>
        /// <returns>All region in a given language - Returned format is "as is" in database</returns>
        /// <exception cref="OnTimeGC_API.Exception.InvalidTokenException"></exception>
        public RegionText RegionText(string languageCode)
        {
            string pd = string.Format("{{{0},{1}}}", this.basis.ToString(), "\"RegionText\":{\"LanguageCode\":\"" + languageCode + "\"}");
            string s = Send(pd, EndPoint.ApiHttp);
            CheckToken(s);

            return s.ToObject<RegionTextResult>().Item;
        }
        #endregion

        #region UsersAll
        /// <summary>
        /// List of all users and their information
        /// </summary>
        /// <returns>List of all users and their information</returns>
        /// <exception cref="OnTimeGC_API.Exception.InvalidTokenException"></exception>
        /// <exception cref="OnTimeGC_API.Exception.InvalidApiResponseException"></exception>
        public List<UsersAllItem> UsersAll()
        {
            string pd = string.Format("{{{0},{1}}}", this.basis.ToString(), "\"UsersAll\":{}");
            string s = Send(pd, EndPoint.ApiHttp);
            CheckToken(s);

            Regex pat = new Regex("\"UsersAll\":(?<m>(.*)),\"Status\""); // "GroupList":((.*)),"Status"
            if (!pat.Match(s).Success)
                throw new InvalidApiResponseException("OnTimeGC API - Invalid API-Response", new System.Exception("API Result: " + s));

            string src = pat.Match(s).Groups["m"].Value;
            string dest = "[" + src.Replace("\"Users\":{", "").TrimStart('{').TrimEnd('}') + "}]";
            dest = Regex.Replace(dest, "\"[^\"]*\":{", "{");
            s = s.Replace(src, dest);

            return s.ToObject<UsersAllResult>().Items;
        }
        #endregion

        #region UsersInfo
        /// <summary>
        /// Information of specific users
        /// </summary>
        /// <param name="ids">IDs for which it should return user info</param>
        /// <returns>List of information of specific users</returns>
        /// <exception cref="OnTimeGC_API.Exception.InvalidTokenException"></exception>
        /// <exception cref="OnTimeGC_API.Exception.InvalidApiResponseException"></exception>
        public List<UsersInfoItem> UsersInfo(string[] ids)
        {
            string pd = string.Format("{{{0},{1}}}", this.basis.ToString(), "\"UsersInfo\":{\"IDs\":[\"" + string.Join("\",\"", ids) + "\"],\"Emails\":null,\"DNs\":null,\"ExcludeIDs\":null,\"Groups\":null,\"Items\":null,\"IncludeNoAccess\":false,\"ShortKeys\":false}");
            string s = Send(pd, EndPoint.ApiHttp);
            CheckToken(s);

            Regex pat = new Regex("\"UsersInfo\":(?<m>(.*)),\"Status\"");
            if (!pat.Match(s).Success || s.Contains("fo\":{\"Users\":{}},\"St"))
                throw new InvalidApiResponseException("OnTimeGC API - Invalid API-Response", new System.Exception("API Result: " + s));

            string src = pat.Match(s).Groups["m"].Value;
            string dest = "[" + src.Replace("\"Users\":{", "").TrimStart('{').TrimEnd('}') + "}]";
            dest = Regex.Replace(dest, "\"[^\"]*\":{", "{");
            s = s.Replace(src, dest);

            return s.ToObject<UsersInfoResult>().Items;
        }
        #endregion

        #region UsersPhoto
        /// <summary>
        /// Photo of specified users in Base64 format
        /// </summary>
        /// <param name="ids"></param>
        /// <returns>List of photos of specified users in Base64 format</returns>
        /// <exception cref="OnTimeGC_API.Exception.InvalidTokenException"></exception>
        /// <exception cref="OnTimeGC_API.Exception.InvalidApiResponseException"></exception>
        public List<UsersPhotoItem> UsersPhoto(string[] ids)
        {
            string pd = string.Format("{{{0},{1}}}", this.basis.ToString(), "\"UsersPhoto\":{\"IDs\":[\"" + string.Join("\",\"", ids) + "\"]}");
            string s = Send(pd, EndPoint.ApiHttp);
            CheckToken(s);

            Regex pat = new Regex("\"UsersPhoto\":(?<m>(.*)),\"Status\"");
            if (!pat.Match(s).Success || s.Contains("fo\":{\"Photos\":{}},\"St"))
                throw new InvalidApiResponseException("OnTimeGC API - Invalid API-Response", new System.Exception("API Result: " + s));

            string src = pat.Match(s).Groups["m"].Value;
            string dest = "[" + src.Replace("\"Photos\":{", "").TrimStart('{').TrimEnd('}') + "}]";
            dest = Regex.Replace(dest, "\"[^\"]*\":{", "{");
            s = s.Replace(src, dest);

            return s.ToObject<UsersPhotoResult>().Items;
        }
        #endregion

        #region AppointmentCreate
        /// <summary>
        /// Create a appointment
        /// </summary>
        /// <param name="source">An "Appointment" object</param>
        /// <returns>Basic informations about the created appointment</returns>
        /// <exception cref="OnTimeGC_API.Exception.InvalidTokenException"></exception>
        public AppointmentCreateItem AppointmentCreate(Appointment source)
        {
            return AppointmentCreate(source.ToString());
        }

        /// <summary>
        /// Create a AllDayEvent
        /// </summary>
        /// <param name="source">An "AllDayEvent" object</param>
        /// <returns>Basic informations about the created alldayevent</returns>
        /// <exception cref="OnTimeGC_API.Exception.InvalidTokenException"></exception>
        public AppointmentCreateItem AppointmentCreate(AllDayEvent source)
        {
            return AppointmentCreate(source.ToString());
        }

        /// <summary>
        /// Create a Meeting
        /// </summary>
        /// <param name="source">An "Meeting" object</param>
        /// <returns>Basic informations about the created meeting</returns>
        /// <exception cref="OnTimeGC_API.Exception.InvalidTokenException"></exception>
        public AppointmentCreateItem AppointmentCreate(Meeting source)
        {
            return AppointmentCreate(source.ToString());
        }


        private AppointmentCreateItem AppointmentCreate(string appt)
        {
            string pd = string.Format("{{{0},{1}}}", this.basis.ToString(), appt);
            string s = Send(pd, EndPoint.ApiHttp);
            CheckToken(s);

            return s.ToObject<AppointmentCreateResult>().Item;
        }
        #endregion

        #region AppointmentRemove
        /// <summary>
        /// Removes an appointment
        /// </summary>
        /// <param name="userID">OnTime User ID for appointment owner</param>
        /// <param name="unId">The document UnID of the appointment you wish to remove</param>
        /// <param name="lastMod">Last modified timestamp of the appointment - must match the actaual appointment LastMod value</param>
        /// <returns>Basic informations about the removed appointment</returns>
        /// <exception cref="OnTimeGC_API.Exception.InvalidTokenException"></exception>
        public AppointmentRemoveItem AppointmentRemove(string userID, string unId, DateTime lastMod)
        {
            string pd = string.Format("{{{0},{1}}}", this.basis.ToString(), "\"AppointmentRemove\":{\"UserID\":\"" + userID + "\",\"UnID\":\"" + unId + "\",\"LastMod\":" + lastMod.ToOTDateTime() + "}");
            string s = Send(pd, EndPoint.ApiHttp);
            CheckToken(s);

            return s.ToObject<AppointmentRemoveResult>().Item;
        }
        #endregion

        #region AppointmentChange
        /// <summary>
        /// Change an appointment or an allday event - you cannot change a meeting
        /// </summary>
        /// <param name="userId">OnTime User ID to be created for</param>
        /// <param name="unId">The document UnID of the appointment you wish to change</param>
        /// <param name="lastMod">Last modified timestamp of the appointment - must match the actaual appointment LastMod value</param>
        /// <param name="newUserId">OnTime User ID to move the appointment to</param>
        /// <param name="newStartDT">Start Date Time</param>
        /// <param name="newEndDT">End Date Time</param>
        /// <param name="newSubject">Subject on appointment</param>
        /// <param name="newLocation">Location on appointment</param>
        /// <param name="newCategories">Categories on appointment</param>
        /// <param name="newPrivate">Is it a Private appointment?</param>
        /// <param name="newAvialable">Is it a marked available?</param>
        /// <returns>Basic informations about the changed appointment</returns>
        /// <exception cref="OnTimeGC_API.Exception.InvalidTokenException"></exception>
        public AppointmentChangeItem AppointmentChange(string userId, string unId, DateTime lastMod, string newUserId, DateTime newStartDT, DateTime newEndDT, string newSubject, string newLocation, string[] newCategories, bool newPrivate, bool newAvialable)
        {
            string pd = string.Format("{{{0},{1}}}", this.basis.ToString(), "\"AppointmentChange\":{\"UserID\":\"" + userId + "\",\"UnID\":\"" + unId + "\",\"LastMod\":" + lastMod + ",\"NewUserID\":\"" + newUserId + "\",\"NewStartDT\":" + newStartDT.ToOTDateTime() + ",\"NewEndDT\":" + newEndDT.ToOTDateTime() + ",\"NewSubject\":\"" + newSubject + "\",\"NewLocation\":\"" + newLocation + "\",\"NewCategories\":[" + string.Join("\",\"", newCategories) + "],\"NewPrivate\":" + newPrivate + ",\"NewAvailable\":" + newAvialable + "}");
            string s = Send(pd, EndPoint.ApiHttp);
            CheckToken(s);

            return s.ToObject<AppointmentChangeResult>().Item;
        }
        #endregion

        #region FreeTimeSearch
        /// <summary>
        /// Freetime search
        /// </summary>
        /// <param name="ids">IDs for which it should return free time search</param>
        /// <param name="startDate">Start DateTime</param>
        /// <param name="endDate">End DateTime</param>
        /// <param name="duration">Minimum free duration (in sec)</param>
        /// <returns>List of freetimes</returns>
        /// <exception cref="OnTimeGC_API.Exception.InvalidTokenException"></exception>
        /// <exception cref="OnTimeGC_API.Exception.InvalidApiResponseException"></exception>
        public List<FreeTimeSearchItem> FreeTimeSearch(string[] ids, DateTime startDate, DateTime endDate, int duration)
        {
            string pd = string.Format("{{{0},{1}}}", this.basis.ToString(), "\"FreeTimeSearch\":{\"IDs\":[\"" + string.Join("\",\"", ids) + "\"],\"FromDT\":" + startDate.ToOTDateTime() + ",\"EndDT\":" + endDate.ToOTDateTime() + ",\"Duration\":" + duration.ToString() + "}");
            string s = Send(pd, EndPoint.ApiHttp);
            CheckToken(s);

            Regex pat = new Regex("\"FreeTimeSearch\":(?<m>(.*)),\"Status\"");
            if (!pat.Match(s).Success)
                throw new InvalidApiResponseException("OnTimeGC API - Invalid API-Response", new System.Exception("API Result: " + s));

            string src = pat.Match(s).Groups["m"].Value;
            string dest = src.Replace("\"Entries\":", "").TrimStart('{').TrimEnd('}');
            s = s.Replace(src, dest);

            return s.ToObject<FreeTimeSearchResult>().Items;
        }
        #endregion





        #region core functions
        private void CheckToken(string s)
        {
            if (s.Contains("\"TokenNoUser\":true"))
                throw new InvalidTokenException(string.Format("OnTimeGC API - No user with given Token ({0})", this.Token), new System.Exception("API Result: " + s));

            if (s.Contains("\"TokenTimeout\":true"))
                throw new InvalidTokenException(string.Format("OnTimeGC API - The given token is no longer valid ({0})", this.Token), new System.Exception("API Result: " + s));
        }

        private bool AcceptAllCertifications(object sender, X509Certificate certification, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; }

        private bool GetLtpaCookie(string username, string password)
        {
            string result = "";

            string postdata = "Username=" + System.Web.HttpUtility.UrlEncode(username) + "&Password=" + System.Web.HttpUtility.UrlEncode(password) + "&RedirectTo=" + System.Web.HttpUtility.UrlEncode("/names.nsf/$about");
            byte[] data = System.Text.Encoding.UTF8.GetBytes(postdata);

            string url = this.onTineGCUrl.OriginalString.Replace(this.onTineGCUrl.LocalPath, "");
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + "/names.nsf?Login");
            request.CookieContainer = this.cc;
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(AcceptAllCertifications);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(data, 0, data.Length);
            }

            using (WebResponse response = request.GetResponse())
            using (Stream responseStream = response.GetResponseStream())

            using (StreamReader buffer = new StreamReader(responseStream, System.Text.Encoding.UTF8))
            {
                result = buffer.ReadToEnd();
            }

            if (result.Contains("Domino Directory"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private TokenResult GetToken()
        {
            string pd = string.Format("{{{0}}}", this.basis.ToString());
            string s = Send(pd, EndPoint.ApiHttpToken);

            return s.ToObject<TokenResult>();
        }

        private string Send(string postData, EndPoint endPoint)
        {
            //string result = "";
            byte[] data = System.Text.Encoding.ASCII.GetBytes(postData);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(this.onTineGCUrl.OriginalString + endPoint.ToUrl());
            request.CookieContainer = this.cc;
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(AcceptAllCertifications);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(data, 0, data.Length);
            }

            using (WebResponse response = request.GetResponse())
            using (Stream responseStream = response.GetResponseStream())

            using (StreamReader buffer = new StreamReader(responseStream, System.Text.Encoding.UTF8))
            {
                return buffer.ReadToEnd();
            }
        }

        /// <summary>
        /// Internal API Endpoints
        /// </summary>
        internal enum EndPoint
        {
            ApiHttp,
            ApiHttpToken
        }
        #endregion

        /// <summary>
        /// The OnTime Group Calendar token
        /// </summary>
        public string Token
        {
            get { return this.basis.Token; }
            set { this.basis.Token = value; }
        }
    }
}