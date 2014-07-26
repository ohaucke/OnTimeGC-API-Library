using System;
using System.Runtime.Serialization;

namespace OnTimeGC_API.Objects
{
    [DataContract]
    public class Meeting
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

        [DataMember(Name = "RequiredAttendees")]
        internal string[] RequiredAttendees { get; set; }

        [DataMember(Name = "OptionalAttendees")]
        internal string[] OptionalAttendees { get; set; }

        [DataMember(Name = "FYIAttendees")]
        internal string[] FYIAttendees { get; set; }

        [DataMember(Name = "RequiredRooms")]
        internal string[] RequiredRooms { get; set; }

        [DataMember(Name = "RequiredResources")]
        internal string[] RequiredResources { get; set; }

        [DataMember(Name = "InvitationSubject")]
        internal string InvitationSubject { get; set; }

        public Meeting() { }

        public Meeting(string userId, DateTime startDT, DateTime endDT, string subject, string location, string[] categories, bool privat, bool available, string body, string[] requiredAttendees, string[] optionalAttendees, string[] fyiAttendees, string[] requiredRooms, string[] requiredResources, string invitationSubject)
        {
            this.UserID = UserID;
            this.AppointmentType = AppointmentType.Meeting;
            this.StartDT = startDT.ToOTDateTime();
            this.EndDT = endDT.ToOTDateTime();
            this.Subject = subject;
            this.Location = location;
            this.Categories = categories;
            this.Private = privat;
            this.Available = available;
            this.Body = body;
            this.RequiredAttendees = requiredAttendees;
            this.OptionalAttendees = optionalAttendees;
            this.FYIAttendees = fyiAttendees;
            this.RequiredRooms = requiredRooms;
            this.RequiredResources = requiredResources;
            this.InvitationSubject = invitationSubject;
        }

        public override string ToString()
        {
            return string.Format("\"AppointmentCreate\":{0}", this.ToJson());
        }
    }
}
