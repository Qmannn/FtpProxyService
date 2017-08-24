using Newtonsoft.Json;
using System.Collections.Generic;

namespace Proxynet.Models
{
    public class SiteToSaveDto
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("port")]
        public int Port { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("login")]
        public string Login { get; set; }
        
        [JsonProperty("groupIds")]
        public IEnumerable<int> GroupIds { get; set; }
    }
}