using System.IO;

namespace MUtils.CqrsBase {
    public interface ISerializer {
        string Serialize<T>(T input, dynamic setting = null);
        T Deserialize<T>(string input);
    }
}