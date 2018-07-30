namespace MUtils.Cqrs {
    public static class CycleContextExtensions {
        public static T GetItem<T>(this ICycleContext context, string key) {
            var obj = context.GetItem(key);
            return obj is T ? (T)obj : default(T);
        }
    }
}