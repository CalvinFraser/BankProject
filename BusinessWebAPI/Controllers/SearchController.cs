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
    public class SearchController : ApiController
    {
        NetTcpBinding tcp;
        private DataBaseServerInterface.DataBaseServerInterface channel;
        ChannelFactory<DataBaseServerInterface.DataBaseServerInterface> channelFactory;

  

        
        public DataIntermed Post([FromBody] SearchData name)
        {
           
            tcp = new NetTcpBinding();
            string URL = "net.tcp://localhost:8100/DataBaseService";
            channelFactory = new ChannelFactory<DataBaseServerInterface.DataBaseServerInterface>(tcp, URL);
            channel = channelFactory.CreateChannel();

            DataIntermed data = new DataIntermed();
            int total = channel.getNumEntires();
            uint tmpAcct = 0, tmpPin = 0;
            int tmpBal = 0;
            Bitmap tmpIcon;
            string tmpFName = "temp", tmpLName = "temp";

            for (int index_ = 0; index_ < total; index_++)
            {
                channel.GetValuesForEntry(index_, out tmpAcct, out tmpPin, out tmpBal, out tmpFName, out tmpLName, out tmpIcon);
                if (String.Equals(name.searchString, tmpLName, StringComparison.OrdinalIgnoreCase))
                {
                    data.acctNo = tmpAcct;
                    data.pin = tmpPin;
                    data.balance = tmpBal;
                    data.firstName = tmpFName;
                    data.lastName = tmpLName;


                    return data;
                }
               
            }
         
            return data;

        }
        

    }
}
