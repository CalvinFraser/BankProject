using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using BusinessServerInterface;
using Utils;
using DatabaseWebBAPI.Models;
using System.Data.Entity.Infrastructure;

namespace DatabaseWebBAPI
{
    public class AccountController : ApiController
    {
        private accountdbEntities db = new accountdbEntities();



        public IHttpActionResult Get(int id)
        {
            Account data = db.Accounts.Find(id);
            if (data == null)
            {
                return NotFound();
            }
            return Ok(data);

        }

        public IQueryable<Account> Get()
        {
            return db.Accounts;
        }

        public IHttpActionResult Post(Account account)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Accounts.Add(account);
            try
            {
                db.SaveChanges();
            }
            catch(DbUpdateException)
            {
                if(AccountExists(account.Id))
                {
                    return Conflict();
                }
                else
                {
                    return InternalServerError();
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = account.Id }, account);
        }

        


        private bool AccountExists(int id)
        {
            return db.Accounts.Count(e => e.Id == id) > 0;
        }

        /*
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
        */

    }
}
