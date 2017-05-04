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

        [JsonIgnore]
        public string StorageId { get; set; }
    }
}