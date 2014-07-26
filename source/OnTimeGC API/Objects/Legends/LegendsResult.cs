using System.Collections.Generic;
using System.Runtime.Serialization;

namespace OnTimeGC_API.Objects
{
    [DataContract]
    internal class LegendsResult : Base
    {
        [DataMember(Name = "Legends")]
        internal LegendsItem Item { get; set; }
    }

    [DataContract]
    public class LegendsItem
    {
        [DataMember(Name = "LanguageCode")]
        public string LanguageCode { get; set; }

        [DataMember(Name = "Items")]
        public List<Legend> Items { get; set; }
    }

    [DataContract]
    public class Legend
    {
        [DataMember(Name = "ID")]
        public int ID { get; set; }

        [DataMember(Name = "BGColor")]
        public string BGColor { get; set; }

        [DataMember(Name = "FGColor")]
        public string FGColor { get; set; }

        [DataMember(Name = "Text")]
        public string Text { get; set; }
    }
}
