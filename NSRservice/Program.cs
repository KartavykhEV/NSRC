using System; 
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Security;
using Microsoft.Extensions.Configuration;
using NSRcommon;

namespace NSRservice
{
    class Program
    {
        static internal String serverName;
        static internal String host;


        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();

            serverName = config["name"];
            host = config["host"];

            callConnect();

            Console.WriteLine(serverName); 

            //Console.WriteLine("Hello World!"); 
        }




        static void callConnect()
        {
            BasicHttpBinding basicHttpBinding = null;
            EndpointAddress endpointAddress = null;
            ChannelFactory<INSRCservice> factory = null;
            INSRCservice serviceProxy = null;

            try
            {
                basicHttpBinding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
                basicHttpBinding.Security.Mode = BasicHttpSecurityMode.None;
                endpointAddress = new EndpointAddress(new Uri($"http://{host}:5001/nsrc_service.asmx"));
                factory = new ChannelFactory<INSRCservice>(basicHttpBinding, endpointAddress);
                serviceProxy = factory.CreateChannel();

                //using (var scope = new OperationContextScope((IContextChannel)serviceProxy))
                //{
                    var result = serviceProxy.Connect(serverName);
                //}

                factory.Close();
                ((ICommunicationObject)serviceProxy).Close();
            }
            catch (MessageSecurityException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                // *** ENSURE CLEANUP (this code is at the WCF GitHub page *** \\
                
            }
        }



    }
}
