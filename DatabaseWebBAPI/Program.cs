using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;
using System;
using System.Net.Http;


namespace DatabaseWebBAPI
{
    public class Program
    {
        
        static void Main(string[] args)
        {
            string baseAddress = "http://localhost:9089/";
            using (WebApp.Start<Startup>(url: baseAddress))
            {
                Console.WriteLine("Data API started.");
                Console.ReadLine();
            }


        }
    }
}
