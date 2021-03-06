﻿using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

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

        [JsonProperty("groups")]
        public List<GroupDto> Groups { get; set; }

        [JsonProperty("account")]
        public UserAccountDto Account { get; set; }
    }
}