using Newtonsoft.Json;

namespace Proxynet.Models
{
    public class UserAccountDto
    {
        [JsonProperty("login")]
        public string Login { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("needSaveAccount")]
        public bool NeedSaveAccount { get; set; }
    }
}