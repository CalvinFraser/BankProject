using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RestSharp;
using DatatierWeb.Models;
using Newtonsoft.Json;
using System.Drawing;
using Utils;

namespace BusinessWebAPI.Controllers
{
    public class GenerateController : ApiController
    {

        private readonly Random _random = new Random();
        private readonly string[] firstNames = { "James", "Katheryn", "Calvin", "Ethan", "Sarah", "Chris" };
        private readonly string[] lastNames = { "Fraser", "Smith", "Douglas", "Janson", "Wilson", "Anderson", "Brown", "Jones", "Williams", "Garcia", "Miller", "Davis", "Martinez", "Lopez", "Thomas", "Anderson" };
        private readonly List<Bitmap> icons = new List<Bitmap>();

        public IHttpActionResult Post()
        {

            Bitmap bmp;
            int width = 64;
            int height = 64;

            for (int i = 0; i < 32; i++)
            {
                bmp = new Bitmap(width, height);
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        int a = _random.Next(256);
                        int r = _random.Next(256);
                        int g = _random.Next(256);
                        int b = _random.Next(256);

                        bmp.SetPixel(x, y, Color.FromArgb(a, r, g, b));
                    }
                }
                icons.Add(bmp);
            }



            try
            {
                RestClient RC = new RestClient("http://localhost:62331/");
                for (int i = 0; i < 1000; i++)
                {
                    Account temp = new Account();
                    temp.Id = i;
                    temp.accountNo = _random.Next(10000000, 99999999);
                    temp.balance = _random.Next(-100000, 999999);
                    temp.pin = _random.Next(9999);
                    temp.firstName = firstNames[_random.Next(firstNames.Length)];
                    temp.lastName = lastNames[_random.Next(lastNames.Length)];
                    Bitmap tmp = icons[_random.Next(icons.Count)];
                    temp.icon64 = Util.BitMapToBase64(tmp);

                    string json = JsonConvert.SerializeObject(temp);
                    RestRequest RR = new RestRequest("api/accounts", Method.Post);
                    RR.AddBody(json);
                    RestResponse restResponse = RC.Post(RR);
                    if(restResponse.StatusCode == HttpStatusCode.Conflict)
                    {
                        return BadRequest();
                    }
                }


            }catch(Exception e)
            {
                return BadRequest();
            }

            return Ok(); 
        }

    
    public IHttpActionResult Delete()
        {

            RestClient RC = new RestClient("http://localhost:62331/");
            for (int i = 0; i < 1000; i++)
            {
                try
                {
                    RestRequest RR = new RestRequest("api/accounts/" + i, Method.Delete);
                    RestResponse restResponse = RC.Delete(RR);
                    if (restResponse.StatusCode == HttpStatusCode.BadRequest)
                    {
                        return BadRequest();
                    }
                    else if (restResponse.StatusCode == HttpStatusCode.NotFound)
                    {
                        continue;
                    }
                }
                catch (Exception e)
                {
                    return BadRequest();
                }
            }

            return Ok();
        }

    }
}
