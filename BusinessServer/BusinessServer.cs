using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessServerInterface;
using System.ServiceModel;
using System.Drawing;
using System.Threading;
using System.Runtime.CompilerServices;

namespace BusinessServer
{

   

    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    internal class BusinessServer : BusinessServerInterface.BusinessServerInterface
    {
        private DataBaseServerInterface.DataBaseServerInterface channel;
        ChannelFactory<DataBaseServerInterface.DataBaseServerInterface> channelFactory;
        NetTcpBinding tcp;
        uint logNumber;
        bool serverCall;


        [MethodImpl(MethodImplOptions.Synchronized)]
        private void addLogNumber()
        {
            logNumber++;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        private void LogValuesForEntry(string logmsg)
        {
            Console.Write("Task " + logNumber + ": ");
            Console.WriteLine(logmsg);
            
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        private void LogGetAccountByLastName(string logmsg)
        {
            Console.Write("Task " + logNumber + ": ");
            Console.WriteLine(logmsg);
            
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void LogGetNumEntries(string logmsg)
        {
            Console.Write("Task " + logNumber + ": ");
            Console.WriteLine(logmsg);

        }
        public BusinessServer()
        {
            tcp = new NetTcpBinding();
            string URL = "net.tcp://localhost:8100/DataBaseService";
            channelFactory = new ChannelFactory<DataBaseServerInterface.DataBaseServerInterface>(tcp, URL);
            channel = channelFactory.CreateChannel();
            logNumber = 0;
            serverCall = false; 
        }
        public int getNumEntires()
        {
            if (!serverCall)
            {
                LogGetNumEntries("Client accessing total value of entires.");
            }
            addLogNumber();
            return channel.getNumEntires();
        }

        public void GetValuesForEntry(int index, out uint acctNo, out uint pin, out int bal, out string fName, out string lName, out Bitmap icon)
        {

            if (!serverCall)
            {
                LogValuesForEntry("Client attempting to access data via index[" + index + "].");
                
            }
            try
            {
                channel.GetValuesForEntry(index, out acctNo, out pin, out bal, out fName, out lName, out icon);
                if (!serverCall)
                {
             
                    LogValuesForEntry("Account found at index [" + index + "]. Account number [" + acctNo + "].");
                }
            }
            catch(FaultException<DataBaseServerInterface.IndexOutOfRangeFault> exception)
            {
                channel.GetValuesForEntry(0, out acctNo, out pin, out bal, out fName, out lName, out icon);
                //TO-DO (CALVIN): Make this more secure. Probably pass it's own exception up to the client. Isn't good to give them a random account. 
                Console.WriteLine(exception.Detail.Issue);

                throw new FaultException<AccountNotFoundFault>(new AccountNotFoundFault() { Issue = "Cannot find that account"});
            }

            if(!serverCall)
            {
                addLogNumber();
            }
            
        }

        public void getAccountByLastName(string lastName, out uint acctNo, out uint pin, out int bal, out string fName, out string lName, out Bitmap icon, out int index)
        {
            serverCall = true;
            
            LogGetAccountByLastName("Client attempting to access account via Last Name [ " + lastName + " ].");
            int total = getNumEntires();
            uint tmpAcct = 0, tmpPin = 0;
            int tmpBal = 0;
            Bitmap tmpIcon = null;
            string tmpFName = "temp", tmpLName = "temp";

            for(int index_ = 0; index_ < total; index_++)
            {
                GetValuesForEntry(index_, out tmpAcct, out tmpPin, out tmpBal, out tmpFName, out tmpLName, out tmpIcon);
                if(String.Equals(lastName, tmpLName, StringComparison.OrdinalIgnoreCase))
                {
                    acctNo = tmpAcct;
                    pin = tmpPin;
                    bal = tmpBal; 
                    fName = tmpFName;
                    lName = tmpLName;
                    icon = tmpIcon;
                    index = index_; 
                    serverCall = false;
                    LogGetAccountByLastName("Found account with search term [ " + lastName + " ]. Account number of [" + acctNo + "].");
                    addLogNumber();
                    return; 
                }
              Thread.Sleep(1000);
            }

            channel.GetValuesForEntry(0, out acctNo, out pin, out bal, out fName, out lName, out icon);
            index = 0; 
            LogGetAccountByLastName("Was unable to find account with search term [ " + lastName + " ].");
            addLogNumber();
            serverCall = false;

        }
    }
}
