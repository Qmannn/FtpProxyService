using System.Collections.Generic;
using Newtonsoft.Json;

namespace Proxynet.Models
{
    [JsonObject]
    public class SiteDto
    {
        [JsonProperty( "id" )]
        public int Id { get; set; }

        [JsonProperty( "description" )]
        public string Description { get; set; }

        [JsonProperty( "name" )]
        public string Name { get; set; }

        [JsonProperty( "groups" )]
        public List<GroupDto> Groups { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("port")]
        public int Port { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("login")]
        public string Login { get; set; }
    }
}