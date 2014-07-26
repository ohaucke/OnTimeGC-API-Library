using System.Runtime.Serialization;

namespace OnTimeGC_API.Objects
{
    [DataContract]
    public class LoginItem
    {
        [DataMember(Name = "APIBuild")]
        public int APIBuild { get; set; }

        [DataMember(Name = "Login")]
        public User User { get; set; }

        [DataMember(Name = "Status")]
        public string Status { get; set; }

        [DataMember(Name = "Error")]
        public string Error { get; set; }
    }
}
