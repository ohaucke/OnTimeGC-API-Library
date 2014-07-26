using System.Collections.Generic;
using System.Runtime.Serialization;

namespace OnTimeGC_API.Objects
{
    [DataContract]
    internal class UsersPhotoResult : Base
    {
        [DataMember(Name = "UsersPhoto")]
        internal List<UsersPhotoItem> Items { get; set; }
    }

    [DataContract]
    public class UsersPhotoItem
    {
        [DataMember(Name = "Extension")]
        public string Extension { get; set; }

        [DataMember(Name = "Pic")]
        public string Pic { get; set; }
    }
}
