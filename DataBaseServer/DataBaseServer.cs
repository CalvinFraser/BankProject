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
using DatabaseLib;
using DataBaseServerInterface;
using System.Drawing;

namespace DataBaseServer
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    internal class DataBaseServer : DataBaseServerInterface.DataBaseServerInterface
    {
        private readonly DataBase dataBase = new DataBase();
        public int getNumEntires()
        {
            return dataBase.GetNumRecords();
        }

        public void GetValuesForEntry(int index, out uint acctNo, out uint pin, out int bal, out string fName, out string lName, out Bitmap icon)
        {
            if(index < 0 || index >= dataBase.GetNumRecords() )
            {
                Console.WriteLine("ERROR: ACCESS ERROR: CLIENT ATTEMPTED TO ACCESS INDEX THAT IS OUT OF RANGE.");

                throw new FaultException<IndexOutOfRangeFault>(new IndexOutOfRangeFault() { Issue = "Client accessed index out of range" });
            }
            

            acctNo = dataBase.GetAcctNoByIndex(index);
            pin = dataBase.GetPINByIndex(index);
            bal = dataBase.GetBalanceByIndex(index);
            fName = dataBase.GetFirstNameByIndex(index);
            lName = dataBase.GetLastNameByIndex(index);
            icon = new Bitmap(dataBase.getIconByIndex(index));
            Console.WriteLine("Index of [" + index + "] account number is: " + acctNo);

        }

        public DataBaseServer() { }
    }
}
