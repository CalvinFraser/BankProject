using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BusinessServerInterface;
using System.ServiceModel;
using System.Drawing;

namespace BusinessWebAPI.Controllers
{
    public class GetAccountController : ApiController
    {
        NetTcpBinding tcp;
        private DataBaseServerInterface.DataBaseServerInterface channel;
        ChannelFactory<DataBaseServerInterface.DataBaseServerInterface> channelFactory;

        public DataIntermed Get(int id)
        {
            tcp = new NetTcpBinding();
            string URL = "net.tcp://localhost:8100/DataBaseService";
            channelFactory = new ChannelFactory<DataBaseServerInterface.DataBaseServerInterface>(tcp, URL);
            channel = channelFactory.CreateChannel();

            DataIntermed data = new DataIntermed();
            Bitmap temp; 
            channel.GetValuesForEntry(id, out data.acctNo, out data.pin, out data.balance, out data.firstName, out data.lastName, out temp);

            return data; 
        }
    }
}
