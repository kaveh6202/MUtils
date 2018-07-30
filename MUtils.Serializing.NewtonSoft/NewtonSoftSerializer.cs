using System;
using MUtils.Interface;
using MUtils.Interface.ConfigurationModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace MUtils.Serializing.NewtonSoft
{
    public class NewtonSoftSerializer : ISerializer
    {
        public string Serialize<T>(T input, SerializerConfig configuration)
        {
            var sting = new JsonSerializerSettings()
            {
                //NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateTimeZoneHandling = DateTimeZoneHandling.Local,
                DateFormatString = "yyyy-MM-ddTHH:mm:ss.000",
            };
            //todo use configutation
            return JsonConvert.SerializeObject(input, sting);
        }

        public T Deserialize<T>(string input, SerializerConfig configuration)
        {
            //todo use configutation
            return JsonConvert.DeserializeObject<T>(input);
        }

        public dynamic DynamicDeserialize(string json)
        {
            return JObject.Parse(json);
        }

        public object DeserializeObject(string input, SerializerConfig configuration = null)
        {
            var sting = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
            };
            return JsonConvert.DeserializeObject(input, sting);
        }
    }
}
