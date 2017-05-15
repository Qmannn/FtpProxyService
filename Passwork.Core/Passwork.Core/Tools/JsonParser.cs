using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Passwork.Core.Tools
{
    class JsonParser : Passwork.Core.Tools.IJsonParser
    {
        public T Parse<T>(string json)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        }

        public object Parse(string json)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject(json);
        }

        public string Serialize(object data)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented);
        }

    }
}
