using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using MUtils.MessageBroker.Model;

namespace MUtils.MessageBroker.Helper
{
    public class MqProviderHelper
    {
        public static ConcurrentQueue<Server> Servers = new ConcurrentQueue<Server>();

        public MqProviderHelper(IEnumerable<Server> servers)
        {
            servers.ToList().ForEach(i => Servers.Enqueue(i));
        }

        public Server Server
        {
            get
            {
                if (!Servers.TryDequeue(out Server value))
                    throw new System.Exception("no server found");
                Servers.Enqueue(value);
                return value;
            }
        }
    }
}