using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TelestaffProxyController : ControllerBase
    {

        // We will use this to access out secrets - We are using Secret Manager
        //  https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-5.0&tabs=linux
        // private IConfiguration Configuration { get; }
        private readonly IConfiguration _config;

        private readonly ILogger<TelestaffProxyController> _logger;

        public TelestaffProxyController(IConfiguration configuration, ILogger<TelestaffProxyController> logger)
        {
            _config = configuration;
            Console.WriteLine(_config["Telestaff:Username"]);
            _logger = logger;
        }

        // public TelestaffProxyController(ILogger<TelestaffProxyController> logger)
        // {
        //     _logger = logger;
        // }

        // This can be updated later to provide better logic or tap directly into telestaff
        private static string fetchRoster(string tsUser, string tsPass, string dUser, string dPass, string date = "")
        {

            string baseUrl = "https://data.webstaff.xyz/roster/" + date;
            // Console.WriteLine(_config);
            // var test = Configuration.GetSection("Telestaff").Get("Username");
            
            //             _TelestaffUsername = 

            string userAgent = "(Stack Weather, kt3i.com)";
            string rawTelestaffData= "{\"status_code\": 500, \"data\": \"Something went wrong.\"}";

            try
            {
                WebClient client = new WebClient();
                client.Headers.Add("User-agent", userAgent);
                client.QueryString.Add("duser", dUser);
                client.QueryString.Add("dpass", dPass);
                client.QueryString.Add("username", tsUser);
                client.QueryString.Add("password", tsPass);

                byte[] data = client.UploadValues(baseUrl, "POST", client.QueryString);

                rawTelestaffData = UnicodeEncoding.UTF8.GetString(data);
                
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return rawTelestaffData;
        }

        [HttpGet]
        public string Get()
        {
            Console.WriteLine(_config["Telestaff:Username"]);
            return fetchRoster(
                    _config["Telestaff:Username"],
                    _config["Telestaff:Password"],
                    _config["Domain:Username"],
                    _config["Domain:Password"]
                );
        }

        [HttpGet("{date}")]
        public string GetRosterByDate(string date)
        {
            return fetchRoster(
                    _config["Telestaff:Username"],
                    _config["Telestaff:Password"],
                    _config["Domain:Username"],
                    _config["Domain:Password"],
                    date
                );
        }
    }
}
