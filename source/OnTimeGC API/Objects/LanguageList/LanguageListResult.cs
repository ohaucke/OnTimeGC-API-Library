using System.Collections.Generic;
using System.Runtime.Serialization;

namespace OnTimeGC_API.Objects
{
    [DataContract]
    internal class LanguageListResult : Base
    {
        [DataMember(Name = "LanguageList")]
        internal List<LanguageListItem> Items { get; set; }
    }

    [DataContract]
    public class LanguageListItem
    {
        [DataMember(Name = "Code")]
        public string Code { get; set; }

        [DataMember(Name = "Name")]
        public string Name { get; set; }

        [DataMember(Name = "LastMod")]
        public int LastMod { get; set; }
    }
}
