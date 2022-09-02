/* CALVIN FRASER
 * CURTIN UNIVERSITY
 * 19921792
 */


using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLib
{
    public class DataBase
    {
        List<DataStruct> dataStructs; 

        public DataBase()
        {
            dataStructs = new List<DataStruct>();
            var Datagen = new DatabaseGen();
            int toGenerate = 1000; 
            for(int i = 0; i < toGenerate; i++)
            {
                DataStruct tmp = new DataStruct();
                Datagen.getNextAccount(out tmp.pin, out tmp.acctNo, out tmp.firstName, out tmp.lastName, out tmp.balance, out tmp.icon);
                dataStructs.Add(tmp);
            }
        }

        public uint GetAcctNoByIndex(int index) { return dataStructs[index].acctNo; }
        public uint GetPINByIndex(int index) { return dataStructs[index].pin; }
        public string GetFirstNameByIndex(int index) { return dataStructs[index].firstName; }
        public string GetLastNameByIndex(int index) { return dataStructs[index].lastName; }

        public Bitmap getIconByIndex(int index) { return dataStructs[index].icon; }

        public int GetBalanceByIndex(int index) { return dataStructs[index].balance; }
        public int GetNumRecords() { return dataStructs.Count; }

    }
}
