namespace MUtils.Cqrs {
    public interface ICycleContext {
        void AddItem(string key, object value);
        object GetItem(string key);
    }
}