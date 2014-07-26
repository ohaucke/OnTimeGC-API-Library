using System.Runtime.Serialization;

namespace OnTimeGC_API.Objects
{
    [DataContract]
    internal class AppointmentRemoveResult : Base
    {
        [DataMember(Name = "AppointmentRemove")]
        internal AppointmentRemoveItem Item { get; set; }
    }

    [DataContract]
    public class AppointmentRemoveItem
    {
        [DataMember(Name = "Status")]
        public string Status { get; set; }

        [DataMember(Name = "StatusNum")]
        public int StatusNum { get; set; }

        [DataMember(Name = "StatusText")]
        public string StatusText { get; set; }
    }
}