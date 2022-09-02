using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace DatabaseWebBAPI
{
    public class TotalDataValuesController : ApiController 
    {
        public IHttpActionResult Get()
        {
           return Ok(Database.DB.GetNumRecords());
        }


    }
}
