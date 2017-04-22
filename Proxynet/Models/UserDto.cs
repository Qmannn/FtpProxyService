using Newtonsoft.Json;

namespace Proxynet.Models
{
    [JsonObject]
    public class UserDto
    {
        [JsonProperty( "id" )]
        public int Id { get; set; }

        [JsonProperty( "login" )]
        public string Login { get; set; }

        [JsonProperty( "name" )]
        public string Name { get; set; }
    }
}