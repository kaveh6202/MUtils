using System.Collections.Generic;

namespace MUtils.MessageBroker.Model
{
    public class Server
    {
        public Server(string hostName, string userName, string password, string virtualHost)
        {
            HostName = hostName;
            UserName = userName;
            Password = password;
            VirtualHost = virtualHost;
        }

        public Server()
        {
        }

        public string UserName { get; set; }
        public string Password { get; set; }
        public string HostName { get; set; }
        public string VirtualHost { get; set; }
    }
}