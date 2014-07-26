using System.Collections.Generic;
using System.Runtime.Serialization;

namespace OnTimeGC_API.Objects
{
    [DataContract]
    internal class RegionListResult : Base
    {
        [DataMember(Name = "RegionList")]
        internal List<RegionListItem> Items { get; set; }
    }

    [DataContract]
    public class RegionListItem
    {
        [DataMember(Name = "Code")]
        public string Code { get; set; }

        [DataMember(Name = "Name")]
        public string Name { get; set; }

        [DataMember(Name = "LastMod")]
        public int LastMod { get; set; }
    }
}
