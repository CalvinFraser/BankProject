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
using Utils; 

namespace BusinessWebAPI.Controllers
{
    public class SearchController : ApiController
    {

  

        
        public IHttpActionResult Post([FromBody] SearchData name)
        {

            RestClient RC = new RestClient("http://localhost:9089/");
            RestRequest restRequest = new RestRequest("api/TotalDataValues");
            RestResponse RR = RC.Get(restRequest);
            DataIntermed DT;
            int totalVals = int.Parse(RR.Content);

            for(int i = 0; i < totalVals; i++)
            {
                restRequest = new RestRequest("api/GetAccount/" + i);
                try
                {
                    RR = RC.Get(restRequest);

                    DT = JsonConvert.DeserializeObject<DataIntermed>(RR.Content);
                    if (String.Equals(DT.lastName, name.searchString, StringComparison.OrdinalIgnoreCase))
                    {
                        return Ok(DT);
                    }
                }catch(System.Net.Http.HttpRequestException e)
                {
                    return BadRequest(e.Message);
                }
                catch(Exception e)
                {
                    return InternalServerError();
                }
            }

            return NotFound();
        }
        

    }
}
