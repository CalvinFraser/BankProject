using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
namespace BusinessServer
{
    internal class BusinessTier
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Business server starting....");

            ServiceHost host;
            NetTcpBinding tcp = new NetTcpBinding();

            host = new ServiceHost(typeof(BusinessServer));

            //Set the socket to use wildcard IP (Accept any destination IP/Interface as long as it matched the port)
            host.AddServiceEndpoint(typeof(BusinessServerInterface.BusinessServerInterface), tcp, "net.tcp://0.0.0.0:8101/BusinessService");

            host.Open();
            Console.WriteLine("Service operational");
            Console.ReadLine();

            host.Close();
        }
    }
}
