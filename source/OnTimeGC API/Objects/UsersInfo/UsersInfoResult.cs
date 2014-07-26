using System.Collections.Generic;
using System.Runtime.Serialization;

namespace OnTimeGC_API.Objects
{
    [DataContract]
    internal class UsersInfoResult : Base
    {
        [DataMember(Name = "UsersInfo")]
        internal List<UsersInfoItem> Items { get; set; }
    }

    [DataContract]
    public class UsersInfoItem
    {
        [DataMember(Name = "ID")]
        public string ID { get; set; }

        [DataMember(Name = "DispName")]
        public string DisplayName { get; set; }

        [DataMember(Name = "Type")]
        public UsersInfoTyp Type { get; set; }  // 1 = mensch | 2 = room

        [DataMember(Name = "Access")]
        public string Access { get; set; }

        [DataMember(Name = "Name")]
        public string Name { get; set; }

        [DataMember(Name = "Email")]
        public string Email { get; set; }

        [DataMember(Name = "FirstName")]
        public string FirstName { get; set; }

        [DataMember(Name = "LastName")]
        public string LastName { get; set; }

        [DataMember(Name = "ShortName")]
        public string ShortName { get; set; }

        [DataMember(Name = "MailDomain")]
        public string MailDomain { get; set; }

        [DataMember(Name = "Title")]
        public string Title { get; set; }

        [DataMember(Name = "Location")]
        public string Location { get; set; }

        [DataMember(Name = "Department")]
        public string Department { get; set; }

        [DataMember(Name = "Company")]
        public string Company { get; set; }

        [DataMember(Name = "OfficePhone")]
        public string OfficePhone { get; set; }

        [DataMember(Name = "CellPhone")]
        public string CellPhone { get; set; }

        [DataMember(Name = "UserCategories")]
        public string[] UserCategories { get; set; }

        [DataMember(Name = "DefaultDuration")]
        public string DefaultDuration { get; set; }

        [DataMember(Name = "WorkTimes")]
        public int[] WorkTimes { get; set; }

        [DataMember(Name = "Description")]
        public string Description { get; set; }

        [DataMember(Name = "RoomType")]
        public string RoomType { get; set; }

        [DataMember(Name = "Capacity")]
        public string Capacity { get; set; }

        [DataMember(Name = "Site")]
        public string Site { get; set; }

        [DataMember(Name = "Building")]
        public string Building { get; set; }

        [DataMember(Name = "Floor")]
        public string Floor { get; set; }

        [DataMember(Name = "Phone")]
        public string Phone { get; set; }

    }
}
