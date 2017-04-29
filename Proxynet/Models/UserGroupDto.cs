using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Proxynet.Models
{
    [JsonObject]
    public class UserGroupDto
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }
    }
}