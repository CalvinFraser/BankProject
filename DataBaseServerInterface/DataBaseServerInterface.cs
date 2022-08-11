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
using System.Drawing; 


namespace DataBaseServerInterface
{
    [ServiceContract]
    public interface DataBaseServerInterface
    {
       
      
            [OperationContract]
            int getNumEntires();

            [OperationContract]
            [FaultContract(typeof(IndexOutOfRangeFault))]
            void GetValuesForEntry(int index, out uint acctNo, out uint pin, out int bal, out string fName, out string lName, out Bitmap icon);
        
    }
}
