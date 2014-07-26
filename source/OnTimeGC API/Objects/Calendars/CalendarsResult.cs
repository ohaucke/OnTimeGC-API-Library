using System.Collections.Generic;
using System.Runtime.Serialization;

namespace OnTimeGC_API.Objects
{
    [DataContract]
    internal class CalendarsResult : Base
    {
        [DataMember(Name = "Calendars")]
        internal CalendarsItem Item { get; set; }
    }

    [DataContract]
    public class CalendarsItem
    {
        [DataMember(Name = "Users")]
        public List<CalendarUser> Users { get; set; }

        [DataMember(Name = "UsersIDs")]
        public List<string> UsersIDs { get; set; }
    }

    [DataContract]
    public class CalendarUser
    {
        [DataMember(Name = "ID")]
        public string ID { get; set; }

        [DataMember(Name = "Items")]
        public List<Calendar> Items { get; set; }
    }

    [DataContract]
    public class Calendar
    {
        [DataMember(Name = "UnID")]
        public string UnID { get; set; }

        [DataMember(Name = "ApptType")]
        public AppointmentType AppointmentType { get; set; }

        [DataMember(Name = "LastMod")]
        public int LastMod { get; set; }

        [DataMember(Name = "StartDT")]
        public int StartDT { get; set; }

        [DataMember(Name = "EndDT")]
        public int EndDT { get; set; }

        [DataMember(Name = "Subject")]
        public string Subject { get; set; }

        [DataMember(Name = "Location")]
        public string Location { get; set; }

        [DataMember(Name = "Categories")]
        public List<string> Categories { get; set; }

        [DataMember(Name = "ApptUnID")]
        public string ApptUnID { get; set; }

        [DataMember(Name = "Available")]
        public bool Available { get; set; }

        [DataMember(Name = "Private")]
        public bool Private { get; set; }

        [DataMember(Name = "Repeat")]
        public bool Repeat { get; set; }

        [DataMember(Name = "LegendID")]
        public int LegendID { get; set; }

        [DataMember(Name = "TimeOff")]
        public bool TimeOff { get; set; }

        [DataMember(Name = "MyPersonal")]
        public bool MyPersonal { get; set; }

        [DataMember(Name = "Chair")]
        public string Chair { get; set; }

        [DataMember(Name = "SendTo")]
        public List<string> SendTo { get; set; }

        [DataMember(Name = "CopyTo")]
        public List<string> CopyTo { get; set; }

        [DataMember(Name = "Rooms")]
        public List<string> Rooms { get; set; }

        [DataMember(Name = "Resources")]
        public List<string> Resources { get; set; }

        [DataMember(Name = "Invitation")]
        public bool Invitation { get; set; }
    }

}
