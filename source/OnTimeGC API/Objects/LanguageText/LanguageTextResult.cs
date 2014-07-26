using System.Runtime.Serialization;

namespace OnTimeGC_API.Objects
{
    [DataContract]
    internal class LanguageTextResult : Base
    {
        [DataMember(Name = "LanguageText")]
        internal LanguageTextItem Item { get; set; }
    }

    [DataContract]
    public class LanguageTextItem
    {
        [DataMember(Name = "Code")]
        public string Code { get; set; }

        [DataMember(Name = "Name")]
        public string Name { get; set; }

        [DataMember(Name = "LastMod")]
        public int LastMod { get; set; }

        [DataMember(Name = "Text")]
        public string Text { get; set; }
    }
}
