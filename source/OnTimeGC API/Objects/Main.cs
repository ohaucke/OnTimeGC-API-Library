using System.Runtime.Serialization;

namespace OnTimeGC_API.Objects
{
    [DataContract]
    internal class Main
    {
        [DataMember(Name = "APIVer")]
        public int APIVersion = 0;

        [DataMember(Name = "ApplID")]
        public string ApplicationId = "";

        [DataMember(Name = "ApplVer")]
        public string ApplicationVersion = "";

        [DataMember(Name = "Token")]
        public string Token = "";

        public override string ToString()
        {
            return string.Format("\"Main\":{0}", this.ToJson());
        }
    }

    [DataContract]
    internal class TokenResult
    {
        [DataMember(Name = "Token")]
        internal string Token { get; set; }

        [DataMember(Name = "IsAnonymous")]
        internal bool IsAnonymous { get; set; }

        [DataMember(Name = "Status")]
        internal string Status { get; set; }

        [DataMember(Name = "Error")]
        internal string Error { get; set; }
    }

    [DataContract]
    internal class Base
    {
        [DataMember(Name = "APIBuild")]
        internal int APIBuild { get; set; }

        [DataMember(Name = "Status")]
        internal string Status { get; set; }

        [DataMember(Name = "Error")]
        internal string Error { get; set; }
    }

    [DataContract]
    public class User
    {
        [DataMember(Name = "ID")]
        public string ID { get; set; }

        [DataMember(Name = "Name")]
        public string Name { get; set; }
    }
}
