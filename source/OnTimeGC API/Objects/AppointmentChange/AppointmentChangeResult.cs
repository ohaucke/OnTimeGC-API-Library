using System.Runtime.Serialization;

namespace OnTimeGC_API.Objects
{
    [DataContract]
    internal class AppointmentChangeResult : Base
    {
        [DataMember(Name = "AppointmentChange")]
        internal AppointmentChangeItem Item { get; set; }
    }

    [DataContract]
    public class AppointmentChangeItem
    {
        [DataMember(Name = "UnID")]
        public string UnID { get; set; }

        [DataMember(Name = "LastMod")]
        public int LastMod { get; set; }

        [DataMember(Name = "Status")]
        public string Status { get; set; }

        [DataMember(Name = "StatusNum")]
        public int StatusNum { get; set; }

        [DataMember(Name = "StatusText")]
        public string StatusText { get; set; }

        [DataMember(Name = "Info")]
        public int Info { get; set; }

        [DataMember(Name = "InfoText")]
        public string InfoText { get; set; }
    }
}