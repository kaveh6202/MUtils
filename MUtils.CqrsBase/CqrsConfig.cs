using System;

namespace MUtils.CqrsBase {
    public class CqrsConfig {
        #region singleton impl
        private static readonly Lazy<CqrsConfig> _instance
            = new Lazy<CqrsConfig>(() => new CqrsConfig());
        public static CqrsConfig Default => _instance.Value;
        private CqrsConfig() { }
        #endregion

        public Func<object,string> Serializer { get; private set; }
        public Func<string,object> Deserializer { get; private set; }

        public CqrsConfig UseSerializer(Func<object, string> func) {
            Serializer = func;
            return this;
        }

        public CqrsConfig UseDeserializer(Func<string, object> func) {
            Deserializer = func;
            return this;
        }
        
    }
}