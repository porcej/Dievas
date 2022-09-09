using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.IO;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Dievas.Controllers {

    [ApiController]
    [Route("api/[controller]")]
    public class TelestaffProxyController : ControllerBase {

        private readonly IConfiguration _config;

        private readonly ILogger<TelestaffProxyController> _logger;

        public TelestaffProxyController(IConfiguration configuration,
                                        ILogger<TelestaffProxyController> logger) {
            _config = configuration;
            _logger = logger;
        }

        // This can be updated later to provide better logic or tap directly into telestaff
        private static string fetchRoster(string baseUrl, string tsUser, string tsPass, 
                                          string dUser, string dPass, string date = "", 
                                          string userAgent = "") {
            baseUrl = $"{baseUrl}{date}";
            string rawTelestaffData= "{\"status_code\": 500, \"data\": \"Something went wrong.\"}";
            try {
                WebClient _client = new WebClient();
                _client.Headers.Add("User-agent", userAgent);
                _client.QueryString.Add("duser", dUser);
                _client.QueryString.Add("dpass", dPass);
                _client.QueryString.Add("username", tsUser);
                _client.QueryString.Add("password", tsPass);

                byte[] data = _client.UploadValues(baseUrl, "POST", _client.QueryString);


                rawTelestaffData = UnicodeEncoding.UTF8.GetString(data);
                
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }
            return rawTelestaffData;
        }

        [HttpGet]
        public string Get() {
            return fetchRoster(
                    baseUrl: _config["Telestaff:Url"],
                    tsUser: _config["Telestaff:Username"],
                    tsPass: _config["Telestaff:Password"],
                    dUser: _config["Domain:Username"],
                    dPass: _config["Domain:Password"],
                    userAgent: _config["Telestaff:User-agent"]
                );
        }

        [HttpGet("{date}")]
        public string GetRosterByDate(string date) {
            return fetchRoster(
                    baseUrl: _config["Telestaff:Url"],
                    tsUser: _config["Telestaff:Username"],
                    tsPass: _config["Telestaff:Password"],
                    dUser: _config["Domain:Username"],
                    dPass: _config["Domain:Password"],
                    date: date,
                    userAgent: _config["Telestaff:User-agent"]
                );
        }
    }
}
