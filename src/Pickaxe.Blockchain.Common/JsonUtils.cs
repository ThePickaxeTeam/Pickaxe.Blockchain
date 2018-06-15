using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Pickaxe.Blockchain.Common
{
    public static class JsonUtils
    {
        public static string Serialize(object value, bool formatted = true)
        {
            string output = JsonConvert.SerializeObject(
                value,
                formatted ? Formatting.Indented : Formatting.None,
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    NullValueHandling = NullValueHandling.Ignore
                });

            return output;
        }
    }
}
