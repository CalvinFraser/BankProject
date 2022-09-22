using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using DatatierWeb.Models;

namespace DatatierWeb.Controllers
{
    public class AccountsController : ApiController
    {
        private databaseEntities db = new databaseEntities();

        // GET: api/Accounts
        public IQueryable<Account> GetAccounts()
        {
            return db.Accounts;
        }

        // GET: api/Accounts/5
        [ResponseType(typeof(Account))]
        public IHttpActionResult GetAccount(int id)
        {
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return NotFound();
            }

            return Ok(account);
        }

        // PUT: api/Accounts/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAccount(Account account)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Entry(account).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountExists(account.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Accounts
        [ResponseType(typeof(Account))]
        public IHttpActionResult PostAccount(Account account)
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
            catch (DbUpdateException)
            {
                if (AccountExists(account.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = account.Id }, account);
        }

        // DELETE: api/Accounts/5
        [ResponseType(typeof(Account))]
        public IHttpActionResult DeleteAccount(int id)
        {
            if(db.Accounts.Count(e => e.Id >= 0) <= 0)
            {
                return BadRequest();
            }

            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return NotFound();
            }
            

            db.Accounts.Remove(account);
            db.SaveChanges();

            return Ok(account);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AccountExists(int id)
        {
            return db.Accounts.Count(e => e.Id == id) > 0;
        }
    }
}