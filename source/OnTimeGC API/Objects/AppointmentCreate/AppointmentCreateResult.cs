using System.Runtime.Serialization;

namespace OnTimeGC_API.Objects
{
    [DataContract]
    internal class AppointmentCreateResult : Base
    {
        [DataMember(Name = "AppointmentCreate")]
        internal AppointmentCreateItem Item { get; set; }
    }

    [DataContract]
    public class AppointmentCreateItem
    {
        [DataMember(Name = "NewUnID")]
        public string NewUnID { get; set; }

        [DataMember(Name = "NewLastMod")]
        public int NewLastMod { get; set; }

        [DataMember(Name = "Status")]
        public string Status { get; set; }

        [DataMember(Name = "StatusNum")]
        public int StatusNum { get; set; }

        [DataMember(Name = "StatusText")]
        public string StatusText { get; set; }
    }
}
