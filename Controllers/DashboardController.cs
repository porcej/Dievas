using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dievas.Hubs;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using AFD.Dashboard.Models;

namespace Dievas.Controllers {

    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase {

        private readonly IConfiguration _config;

        private readonly IHubContext<DashboardHub, IDashboardHub> _hubContext;

        private readonly ILogger<DashboardController> _logger;

        private static CAD _cad;

        public DashboardController(IHubContext<DashboardHub, IDashboardHub> dashboardHub,
                                   IConfiguration configuration,
                                   ILogger<DashboardController> logger,
                                   CAD cad) {

            _hubContext = dashboardHub;
            _config = configuration;
            _logger = logger;
            _cad = cad;
        }

        // Readonly web API for incidents information
        [HttpGet("incidents")]
        public IEnumerable GetIncidents() {
            return _cad.GetIncidents();
        }

        [HttpGet("incidents/active/")]
        public IEnumerable GetActiveIncidents() {
            return _cad.GetActiveIncidents();
        }

        [HttpGet("incident/{id}")]
        public ActionResult GetIncident(int id) {
            return new JsonResult(_cad.GetIncident(id));
        }

        [HttpGet("units")]
        public IEnumerable GetUnits()
        {
            return _cad.GetUnits();
        }

        // Web API for data feeds to add incident information

        // Add (or updates) new incident
        [HttpPost("feed/incident")]
        [Authorize]
        public async Task<IncidentDto> AddIncident(IncidentDto incident) {
            await this._hubContext.Clients.Group("dashboard").IncidentAdded(incident);
            return _cad.AddIncident(incident);
        }

        // Add existing incident by id
        [HttpPost("feed/incident/{id}")]
        [Authorize]
        public async Task<IncidentDto> AddIncidentById(int id, IncidentDto incident) {
            await this._hubContext.Clients.Group("dashboard").IncidentAdded(incident);
            return _cad.AddIncident(id, incident);
        }

        // // Update existing incident
        [HttpPost("feed/incident/{id}/update")]
        [Authorize]
        public async Task<IncidentDto> UpdateIncident(int id, string field, string value) {
            await this._hubContext
                      .Clients
                      .Group(id.ToString())
                      .IncidentFieldChanged(id, field, value);
            return _cad.UpdateIncidentField(id, field, value);
        }

        // Add or update unit on incident
        [HttpPost("feed/incident/{id}/unit")]
        [Authorize]
        public async Task<IncidentDto> UpdateIncidentUnits(int id, UnitAssignmentDto unit) {
            await this._hubContext
                        .Clients
                        .Group(id.ToString())
                        .IncidentUnitStatusChanged(id, unit);

            return _cad.AddOrUpdateIncidentUnit(id, unit);
        }

        [HttpPost("feed/incident/{id}/comment")]
        [Authorize]
        public async Task<IncidentDto> AddIncidentComment(int id, CommentDto comment) {
            await this._hubContext
                        .Clients
                        .Group(id.ToString())
                        .IncidentCommentAdded(id, comment);

            return _cad.AddOrUpdateIncidentComment(id, comment);
        }

        // TEST FUNCTIONS PLEASE REMOVE
        //[HttpGet("test")]
        //public async Task<IncidentDto> GenerateTestIncident() {
        //    var _nextCount = _cad.IncidentCount() + 1;
        //    var _incident = new IncidentDto {
        //        Id = _nextCount,
        //        active = true,
        //        jurisdiction = "200 ALX",
        //        incidentType = "FIRE-LOCAL ALARM",
        //        LocationName = "ALEXANDRIA FIRE STATION 204",
        //        address = _nextCount.ToString(),
        //        apartment = "",
        //        city = "CITY OF ALEXANDRIA",
        //        state = "VA",
        //        postalCode = "22314",
        //        county = "Alexandria",
        //        locationType = "Government or Public Building",
        //        longitude = -77.0467847,
        //        latitude = 38.8163701,
        //        crossStreet = "POWHATAN ST",
        //        commandChannel = "",
        //        primaryTACChannel = "2 BRAVO",
        //        alternateTACChannel = "",
        //        callDisposition = "",
        //        incidentStartTime = DateTime.Now,
        //        // incidentEndTime = "",
        //        Comments = new List<CommentDto>{ new CommentDto { id = 0, commentText = "incident notes here"}},
        //        UnitsAssigned = new List<UnitAssignmentDto>{ new UnitAssignmentDto { radioName = "E204", statusId = 1 } }
        //    };

        //    return await this.AddIncidentById(_nextCount, _incident);
        //}

        [HttpGet("testUpdate")]
        public async Task<IncidentDto> GenerateTestUpdate() {
            var _incidentId = 1;
            var _field = "incidentType";
            var _value = "* UPDATED * ";
            return await this.UpdateIncident(_incidentId, _field, _value);
        }

        [HttpGet("testUpdate/{id}")]
        public async Task<IncidentDto> GenerateTestUpdate(int id) {
            var _field = "incidentType";
            var _value = "* UPDATED * ";
            return await this.UpdateIncident(id, _field, _value);
        }

        [HttpGet("testUnit")]
        public async Task<IncidentDto> GenerateTestUnit() {
            var _id = 1;
            var _unit = new UnitAssignmentDto { RadioName = "*200*", StatusId = 1 };
            return await this.UpdateIncidentUnits(_id, _unit);
        }

        [HttpGet("testUnitUpdate/{unit}")]
        public async Task<IncidentDto> GenerateTestUnitUpdate(string unit) {
            var _id = 1;
            var _unit = new UnitAssignmentDto { RadioName = unit, StatusId = 5 };
            return await this.UpdateIncidentUnits(_id, _unit);
        }

        
    }
}