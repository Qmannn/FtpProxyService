using System.Runtime.Serialization;

namespace Proxynet.Models
{
    [DataContract]
    public class UserGroupDto
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "id")]
        public int Id { get; set; }
    }
}