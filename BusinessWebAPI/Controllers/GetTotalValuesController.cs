using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;

using RestSharp;

namespace BusinessWebAPI.Controllers
{
    public class GetTotalValuesController : ApiController
    {





        // GET api/GetTotalValues
        public IHttpActionResult Get()
        {

            
            RestClient RC = new RestClient("http://localhost:9089/");
            RestRequest RR = new RestRequest("api/TotalDataValues");
            RestResponse restResponse = RC.Get(RR);
            int values; 
            if(int.TryParse(restResponse.Content, out values))
            {
                return Ok(values);
            }
            return InternalServerError();
            
        }

    }
}
