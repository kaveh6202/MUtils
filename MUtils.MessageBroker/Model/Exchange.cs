using System.Collections.Generic;

namespace MUtils.MessageBroker.Model
{
    public class Exchange
    {
        public string Name { get; set; }
        public bool Durable { get; set; }
        public bool AutoDelete { get; set; }
        public string Type { get; set; }
        public IDictionary<string, object> Options { get; set; }
    }
}