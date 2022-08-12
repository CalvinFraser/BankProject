using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessServerInterface;
using System.ServiceModel;
using System.Drawing;

namespace BusinessServer
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    internal class BusinessServer : BusinessServerInterface.BusinessServerInterface
    {
        private DataBaseServerInterface.DataBaseServerInterface channel;
        ChannelFactory<DataBaseServerInterface.DataBaseServerInterface> channelFactory;
        NetTcpBinding tcp;


        public BusinessServer()
        {
            tcp = new NetTcpBinding();
            string URL = "net.tcp://localhost:8100/DataBaseService";
            channelFactory = new ChannelFactory<DataBaseServerInterface.DataBaseServerInterface>(tcp, URL);
            channel = channelFactory.CreateChannel();
        }
        public int getNumEntires()
        {
            return channel.getNumEntires();
        }

        public void GetValuesForEntry(int index, out uint acctNo, out uint pin, out int bal, out string fName, out string lName, out Bitmap icon)
        {
            try
            {
                channel.GetValuesForEntry(index, out acctNo, out pin, out bal, out fName, out lName, out icon);
            }catch(FaultException<DataBaseServerInterface.IndexOutOfRangeFault> exception)
            {
                channel.GetValuesForEntry(0, out acctNo, out pin, out bal, out fName, out lName, out icon);
                //TO-DO (CALVIN): Make this more secure. Probably pass it's own exception up to the client. Isn't good to give them a random account. 
                Console.WriteLine(exception.Detail.Issue);

                throw new FaultException<AccountNotFoundFault>(new AccountNotFoundFault() { Issue = "Cannot find that account"});
            }
   
        }

        public void getAccountByLastName(string lastName, out uint acctNo, out uint pin, out int bal, out string fName, out string lName, out Bitmap icon)
        {
            int total = getNumEntires();

            uint tmpAcct = 0, tmpPin = 0;
            int tmpBal = 0;
            Bitmap tmpIcon = null;
            string tmpFName = "temp", tmpLName = "temp";

            for(int index = 0; index < total; index++)
            {
                GetValuesForEntry(index, out tmpAcct, out tmpPin, out tmpBal, out tmpFName, out tmpLName, out tmpIcon);
                if(String.Equals(lastName, tmpLName, StringComparison.OrdinalIgnoreCase))
                {
                    acctNo = tmpAcct;
                    pin = tmpPin;
                    bal = tmpBal; 
                    fName = tmpFName;
                    lName = tmpLName;
                    icon = tmpIcon;
                    return; 
                }

            }

            channel.GetValuesForEntry(0, out acctNo, out pin, out bal, out fName, out lName, out icon);
        }
    }
}
