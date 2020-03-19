using System; 
using System.Collections.Generic; 
using System.Linq; 
using NSRcommon; 

namespace NeuronServerRemoteControl
{
    public class NSRCservice : INSRCservice
    {

        internal static List<NSRCserver> servers = new List<NSRCserver>(); 
        private object serversListLock = new object(); 

        /// <summary>
        /// Сервис для управления подсистемами
        /// </summary>
        public NSRCservice()
        {



        }

        public NSRCcommand Connect(String name)
        {
            //Console.WriteLine($"Connected server '{name}'");
            NSRCserver server = servers.FirstOrDefault(i => i.name.Equals(name, StringComparison.InvariantCultureIgnoreCase)); 
            if (server == null) // нет такого
            {
                server = new NSRCserver() { name = name, lastConnect = DateTime.Now }; 
                lock (serversListLock)
                    servers.Add(server); 
            }

            server.lastConnect = DateTime.Now; 
            return server.GetCommand(); 
        }

        public NSRCcommand SendResponse(String name, NSRCresponse response)
        {
            NSRCserver server = servers.FirstOrDefault(i => i.name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
            server.AddResponse(response);
            return Connect(name);
        }


    }
}
