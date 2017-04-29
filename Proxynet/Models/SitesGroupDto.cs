using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Proxynet.Models
{
    [JsonObject]
    public class SitesGroupDto
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }
    }
}