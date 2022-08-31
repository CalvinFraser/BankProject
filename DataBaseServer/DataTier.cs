/* CALVIN FRASER
 * CURTIN UNIVERSITY
 * 19921792
 */



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using DataBaseServerInterface;
namespace DataBaseServer
{
    internal class DataTier
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Server starting....");
            
            ServiceHost host;
            NetTcpBinding tcp = new NetTcpBinding();

            DataBaseServer DS = new DataBaseServer();

            host = new ServiceHost(DS);

            //Set the socket to use wildcard IP (Accept any destination IP/Interface as long as it matched the port)
            host.AddServiceEndpoint(typeof(DataBaseServerInterface.DataBaseServerInterface), tcp, "net.tcp://0.0.0.0:8100/DataBaseService");

            host.Open();
            Console.WriteLine("Service operational");
            Console.ReadLine();

            host.Close();
        }
    }
}
