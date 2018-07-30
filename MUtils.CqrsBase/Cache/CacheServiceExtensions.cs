namespace MUtils.CqrsBase {
    public static class CacheServiceExtensions {
        public static T Get<T>(this ICacheService service, string key, CacheScope scope = CacheScope.Default) {
            var obj = service.Get(key, scope);
            return obj is T ? (T)obj : default(T);
        }
    }
}