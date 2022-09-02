using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BusinessServerInterface;
using System.ServiceModel;
using System.Drawing;
using Newtonsoft.Json;
using RestSharp;

namespace BusinessWebAPI.Controllers
{
    public class GetAccountController : ApiController
    {


        public IHttpActionResult Get(int id)
        {
            RestClient RC = new RestClient("http://localhost:9089/");
            RestRequest RR = new RestRequest("api/GetAccount/" + id);
            try
            {
                RestResponse restResponse = RC.Get(RR);

                DataIntermed data = JsonConvert.DeserializeObject<DataIntermed>(restResponse.Content);

                return Ok(data);
            }catch(Exception e)
            {
                return InternalServerError(e);
            }
        }
    }
}
