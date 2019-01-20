using MUtils.Interface.ConfigurationModel;

namespace MUtils.Interface
{
    public interface ISerializer
    {
        string Serialize<T>(T data, SerializerConfig configuration = null);
        T Deserialize<T>(string json, SerializerConfig configuration = null);
        object DeserializeObject(string json, SerializerConfig configuration = null);
        dynamic DynamicDeserialize(string json);
    }
}
