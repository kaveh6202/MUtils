using System;

namespace MUtils.CqrsBase {

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class CachePolicyAttribute : Attribute {
        public CacheScope Scope { get; }
        public CacheKeyPolicy KeyPolicy { get; }
        public string KeyPropertyName { get; set; }
        public int LifeSpanInSecond { get; set; }

        public CachePolicyAttribute(CacheScope scope, CacheKeyPolicy policy) {
            Scope = scope;
            KeyPolicy = policy;
        }
    }

}