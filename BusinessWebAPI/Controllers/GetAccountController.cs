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
    public class GetAccountController : ApiController
    {


        public IHttpActionResult Get(int id)
        {
            RestClient RC = new RestClient("http://localhost:62331");
            RestRequest RR = new RestRequest("api/Accounts/" + id);
            try
            {
                RestResponse restResponse = RC.Get(RR);
                if (restResponse.StatusCode == HttpStatusCode.OK)
                {
                    Account data = JsonConvert.DeserializeObject<Account>(restResponse.Content);

                    return Ok(data);
                }
                else if (restResponse.StatusCode == HttpStatusCode.NotFound)
                {
                    return NotFound();

                }
                else
                {
                    return BadRequest();
                }
            }catch(Exception e)
            {
                return InternalServerError(e);
            }
        }

        public IHttpActionResult Delete(int id)
        {
            RestClient RC = new RestClient("http://localhost:62331");
            RestRequest RR = new RestRequest("api/Accounts/" + id);
            try
            {
                RestResponse restResponse = RC.Delete(RR);
                if (restResponse.StatusCode == HttpStatusCode.OK)
                {
                    Account data = JsonConvert.DeserializeObject<Account>(restResponse.Content);

                    return Ok(data);
                }
                else if(restResponse.StatusCode == HttpStatusCode.NotFound)
                {
                    return NotFound();
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        public IHttpActionResult Put(Account account)
        {
            RestClient RC = new RestClient("http://localhost:62331");
            RestRequest RR = new RestRequest("api/Accounts/");


            try
            {
                string acc = JsonConvert.SerializeObject(account);
                RR.AddBody(acc);
                RestResponse restResponse = RC.Put(RR);
                if (restResponse.StatusCode == HttpStatusCode.BadRequest)
                {
                    return BadRequest();
                }
                else if (restResponse.StatusCode == HttpStatusCode.NotFound)
                {
                    return NotFound();
                }
                Account data = JsonConvert.DeserializeObject<Account>(restResponse.Content);

                return Ok(data);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }
        public IHttpActionResult Post(Account account)
        {
            RestClient RC = new RestClient("http://localhost:62331");
            RestRequest RR = new RestRequest("api/Accounts/");

            
            try
            {
                string acc = JsonConvert.SerializeObject(account);
                RR.AddBody(acc);
                RestResponse restResponse = RC.Post(RR);
                if (restResponse.StatusCode == HttpStatusCode.BadRequest)
                {
                    return BadRequest();
                }
                else if (restResponse.StatusCode == HttpStatusCode.Conflict)
                {
                    return Conflict();
                }
                Account data = JsonConvert.DeserializeObject<Account>(restResponse.Content);

                return Ok(data);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }


    }
}
