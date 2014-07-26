using System.Runtime.Serialization;

namespace OnTimeGC_API.Objects
{
    [DataContract]
    public class LogoutItem
    {
        [DataMember(Name = "APIBuild")]
        public int APIBuild { get; set; }

        [DataMember(Name = "Logout")]
        public User User { get; set; }

        [DataMember(Name = "Status")]
        public string Status { get; set; }
    }
}
