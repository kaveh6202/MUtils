using System.Collections.Generic;

namespace MUtils.MessageBroker.Model
{
    public class Queue
    {
        public Queue(string name, bool durable = true, bool exclusive = false, bool autoDelete = false,
            IDictionary<string, object> options = null)
        {
            Name = name;
            Durable = durable;
            Exclusive = exclusive;
            AutoDelete = autoDelete;
            Options = options;
        }

        public string Name { get; set; }
        public bool Durable { get; set; }   
        public bool Exclusive { get; set; }
        public bool AutoDelete { get; set; }
        public IDictionary<string, object> Options { get; set; }
    }
}