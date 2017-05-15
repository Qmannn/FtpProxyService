using System;
namespace Passwork.Core.Tools
{
    interface IJsonParser
    {
        object Parse(string json);
        T Parse<T>(string json);
        string Serialize(object data);
    }
}
