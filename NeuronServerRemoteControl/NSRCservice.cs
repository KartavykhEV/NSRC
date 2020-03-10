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

            /// <summary>
            /// Все подключенные сервера
            /// </summary>



        }

        public NSRCcommand Connect(String name)
        {

            Console.WriteLine($"Connected server '{name}'");


            NSRCserver server = servers.FirstOrDefault(i => i.name.Equals(name, StringComparison.InvariantCultureIgnoreCase)); 
            if (server == null) // нет такого
            {
                server = new NSRCserver() { name = name, lastConnect = DateTime.Now }; 
                lock (serversListLock)
                    servers.Add(server); 
            }

            server.lastConnect = DateTime.Now; 
            return server.Commands.Count>0 ? server.Commands.Dequeue() : null; 
        }

}
}
