using System.Collections.Generic;
using System.Runtime.Serialization;

namespace OnTimeGC_API.Objects
{
    [DataContract]
    internal class FreeTimeSearchResult : Base
    {
        [DataMember(Name = "FreeTimeSearch")]
        internal List<FreeTimeSearchItem> Items { get; set; }
    }

    [DataContract]
    public class FreeTimeSearchItem
    {
        [DataMember(Name = "Start")]
        public int Start { get; set; }

        [DataMember(Name = "End")]
        public int End { get; set; }
    }
}
