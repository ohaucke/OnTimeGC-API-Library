using System;
using System.Runtime.Serialization;

namespace OnTimeGC_API.Objects
{
    [DataContract]
    public class AllDayEvent
    {
        [DataMember(Name = "UserID")]
        public string UserID { get; set; }

        [DataMember(Name = "AppointmentType")]
        public AppointmentType AppointmentType { get; set; }

        [DataMember(Name = "StartDT")]
        public int StartDT { get; set; }

        [DataMember(Name = "EndDT")]
        public int EndDT { get; set; }

        [DataMember(Name = "Subject")]
        public string Subject { get; set; }

        [DataMember(Name = "Location")]
        public string Location { get; set; }

        [DataMember(Name = "Categories")]
        public string[] Categories { get; set; }

        [DataMember(Name = "Private")]
        public bool Private { get; set; }

        [DataMember(Name = "Available")]
        public bool Available { get; set; }

        [DataMember(Name = "Body")]
        public string Body { get; set; }

        internal AllDayEvent() { }

        public AllDayEvent(string userId, DateTime startDT, DateTime endDT, string subject, string location, string[] categories, bool privat, bool available, string body)
        {
            this.UserID = userId;
            this.AppointmentType = AppointmentType.AllDayEvent;
            this.StartDT = startDT.ToOTDateTime();
            this.EndDT = endDT.ToOTDateTime();
            this.Subject = subject;
            this.Location = location;
            this.Categories = categories;
            this.Private = privat;
            this.Available = available;
            this.Body = body;
        }

        public override string ToString()
        {
            return string.Format("\"AppointmentCreate\":{0}", this.ToJson());
        }
    }
}
