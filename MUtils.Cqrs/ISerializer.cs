using System.IO;

namespace MUtils.Cqrs {
    public interface ISerializer {
        string Serialize<T>(T input, dynamic setting = null);
        T Deserialize<T>(string input);
    }
}