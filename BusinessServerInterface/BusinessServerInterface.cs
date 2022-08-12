using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Drawing;

namespace BusinessServerInterface
{
    [ServiceContract]
    public interface BusinessServerInterface
    {


        [OperationContract]
        int getNumEntires();

        [OperationContract]
        [FaultContract(typeof(AccountNotFoundFault))]
        void GetValuesForEntry(int index, out uint acctNo, out uint pin, out int bal, out string fName, out string lName, out Bitmap icon);

        [OperationContract]
        void getAccountByLastName(string lastName, out uint acctNo, out uint pin, out int bal, out string fName, out string lName, out Bitmap icon);
    }
}
