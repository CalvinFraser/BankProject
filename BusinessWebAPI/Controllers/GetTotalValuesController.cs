using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.ServiceModel;
namespace BusinessWebAPI.Controllers
{
    public class GetTotalValuesController : ApiController
    {
        NetTcpBinding tcp;
        private DataBaseServerInterface.DataBaseServerInterface channel;
        ChannelFactory<DataBaseServerInterface.DataBaseServerInterface> channelFactory;




        // GET api/GetTotalValues
        public int Get()
        {
            tcp = new NetTcpBinding();
            string URL = "net.tcp://localhost:8100/DataBaseService";
            channelFactory = new ChannelFactory<DataBaseServerInterface.DataBaseServerInterface>(tcp, URL);
            channel = channelFactory.CreateChannel();

            return channel.getNumEntires();
        }

    }
}
