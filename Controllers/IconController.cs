using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dievas.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Dievas.Controllers {


    /// <summary>
    ///     Controller Class <c>IconController</c> serves up hot icons
    ///     
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class IconController : ControllerBase {

        private readonly IConfiguration _config;

        private readonly ILogger<IconController> _logger;

        public DashboardController(IConfiguration configuration,
                                   ILogger<IconController> logger) {

            _config = configuration;
            _logger = logger;
        }

        // Readonly web API for incidents information
        [HttpGet("marker/{callType}")]
        public IActionResult GetIncidents(string callType) {
            FileStream stream = File.Open(@"");
            return _cad.GetIncidents();
        }
    }
}