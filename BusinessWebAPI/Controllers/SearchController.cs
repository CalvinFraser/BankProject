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
using DatatierWeb.Models;

namespace BusinessWebAPI.Controllers
{
    public class SearchController : ApiController
    {

  

        
        public IHttpActionResult Post([FromBody] SearchData name)
        {

            RestClient RC = new RestClient("http://localhost:62331/");
            RestRequest restRequest;
            RestResponse RR;
            Account DT;
            int i = 0;

            do
            {
                restRequest = new RestRequest("api/accounts/" + i);
                try
                {
                    RR = RC.Get(restRequest);

                    if (RR.StatusCode == HttpStatusCode.OK)
                    {
                        DT = JsonConvert.DeserializeObject<Account>(RR.Content);
                        if (String.Equals(DT.lastName, name.searchString, StringComparison.OrdinalIgnoreCase))
                        {
                            return Ok(DT);
                        }
                    }
                } catch (System.Net.Http.HttpRequestException e)
                {
                    return BadRequest(e.Message);
                }
                catch (Exception e)
                {
                    return InternalServerError();
                }

                i++;
            } while (RR.StatusCode != HttpStatusCode.NotFound);

            return NotFound();
        }
        

    }
}
