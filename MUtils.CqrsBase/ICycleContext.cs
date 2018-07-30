namespace MUtils.CqrsBase {
    public interface ICycleContext {
        void AddItem(string key, object value);
        object GetItem(string key);
    }
}