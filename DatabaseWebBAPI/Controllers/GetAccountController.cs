using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using BusinessServerInterface;
using Utils;
namespace DatabaseWebBAPI
{
    public class GetAccountController : ApiController 
    {
        public IHttpActionResult Get(int id)
        {
            DataIntermed data = new DataIntermed();
            data.acctNo = Database.DB.GetAcctNoByIndex(id);
            data.balance = Database.DB.GetBalanceByIndex(id);
            data.lastName = Database.DB.GetLastNameByIndex(id);
            data.firstName = Database.DB.GetFirstNameByIndex(id);
            data.pin = Database.DB.GetPINByIndex(id);
            Bitmap temp = Database.DB.getIconByIndex(id);
            data.icon64 = Util.BitMapToBase64(temp);

            return Ok(data);

        }

        public IHttpActionResult Get()
        {
            return Ok();
        }
    }
}
