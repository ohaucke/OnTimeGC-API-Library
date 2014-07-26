using System.Runtime.Serialization;

namespace OnTimeGC_API.Objects
{
    [DataContract]
    internal class VersionResult : Base
    {
        [DataMember(Name = "Version")]
        internal Version Item { get; set; }
    }

    [DataContract]
    public class Version
    {
        [DataMember(Name = "APIVersion")]
        public string APIVersion { get; set; }

        [DataMember(Name = "APIBuild")]
        public int APIBuild { get; set; }

        [DataMember(Name = "UserName")]
        public string UserName { get; set; }

        [DataMember(Name = "ServerName")]
        public string ServerName { get; set; }

        [DataMember(Name = "ServerVersion")]
        public string ServerVersion { get; set; }

        [DataMember(Name = "ServerPlatform")]
        public string ServerPlatform { get; set; }
    }
}
