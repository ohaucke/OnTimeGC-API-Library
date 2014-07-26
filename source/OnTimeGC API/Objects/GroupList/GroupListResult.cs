using System.Collections.Generic;
using System.Runtime.Serialization;

namespace OnTimeGC_API.Objects
{
    [DataContract]
    internal class GroupListResult : Base
    {
        [DataMember(Name = "GroupList")]
        internal List<GroupListItem> Items { get; set; }
    }

    [DataContract]
    public class GroupListItem
    {
        [DataMember(Name = "ID")]
        public string ID { get; set; }

        [DataMember(Name = "Name")]
        public string Name { get; set; }

        [DataMember(Name = "IsUser")]
        public bool IsUser { get; set; }
    }
}
