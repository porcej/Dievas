using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Hubs;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Backend.Controllers {

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

        // Web API for data feeds to add incident information

        // Add (or updates) new incident
        [HttpPost("feed/incident")]
        [Authorize]
        public async Task<Incident> AddIncident(Incident incident) {
            await this._hubContext.Clients.Group("dashboard").IncidentAdded(incident);
            return _cad.AddIncident(incident);
        }

        // Add existing incident by id
        [HttpPost("feed/incident/{id}")]
        [Authorize]
        public async Task<Incident> AddIncidentById(int id, Incident incident) {
            await this._hubContext.Clients.Group("dashboard").IncidentAdded(incident);
            return _cad.AddIncident(id, incident);
        }

        // // Update existing incident
        [HttpPost("feed/incident/{id}/update")]
        [Authorize]
        public async Task<Incident> UpdateIncident(int id, string field, string value) {
            await this._hubContext
                      .Clients
                      .Group(id.ToString())
                      .IncidentFieldChanged(id, field, value);
            return _cad.UpdateIncidentField(id, field, value);
        }

        // Add or update unit on incident
        [HttpPost("feed/incident/{id}/unit")]
        [Authorize]
        public async Task<Incident> UpdateIncidentUnits(int id, AssignedUnit unit) {
            await this._hubContext
                        .Clients
                        .Group(id.ToString())
                        .IncidentUnitStatusChanged(id, unit);

            return _cad.AddOrUpdateIncidentUnit(id, unit);
        }

        [HttpPost("feed/incident/{id}/comment")]
        [Authorize]
        public async Task<Incident> AddIncidentComment(int id, Comment comment) {
            await this._hubContext
                        .Clients
                        .Group(id.ToString())
                        .IncidentCommentAdded(id, comment);

            return _cad.AddOrUpdateIncidentComment(id, comment);
        }

        // TEST FUNCTIONS PLEASE REMOVE
        [HttpGet("test")]
        public async Task<Incident> GenerateTestIncident() {
            var _nextCount = _cad.IncidentCount() + 1;
            var _incident = new Incident {
                id = _nextCount,
                active = true,
                jurisdiction = "200 ALX",
                incidentType = "FIRE-LOCAL ALARM",
                LocationName = "ALEXANDRIA FIRE STATION 204",
                address = _nextCount.ToString(),
                apartment = "",
                city = "CITY OF ALEXANDRIA",
                state = "VA",
                postalCode = "22314",
                county = "Alexandria",
                locationType = "Government or Public Building",
                longitude = -77.0467847,
                latitude = 38.8163701,
                crossStreet = "POWHATAN ST",
                commandChannel = "",
                primaryTACChannel = "2 BRAVO",
                alternateTACChannel = "",
                callDisposition = "",
                incidentStartTime = DateTime.Now,
                // incidentEndTime = "",
                Comments = new List<Comment>{ new Comment { id = 0, commentText = "incident notes here"}},
                Units = new List<AssignedUnit>{ new AssignedUnit { radioName = "E204", statusId = 1 } }
            };

            return await this.AddIncidentById(_nextCount, _incident);
        }

        [HttpGet("testUpdate")]
        public async Task<Incident> GenerateTestUpdate() {
            var _incidentId = 1;
            var _field = "incidentType";
            var _value = "* UPDATED * ";
            return await this.UpdateIncident(_incidentId, _field, _value);
        }

        [HttpGet("testUpdate/{id}")]
        public async Task<Incident> GenerateTestUpdate(int id) {
            var _field = "incidentType";
            var _value = "* UPDATED * ";
            return await this.UpdateIncident(id, _field, _value);
        }

        [HttpGet("testUnit")]
        public async Task<Incident> GenerateTestUnit() {
            var _id = 1;
            var _unit = new AssignedUnit { radioName = "*200*", statusId = 1 };
            return await this.UpdateIncidentUnits(_id, _unit);
        }

        [HttpGet("testUnitUpdate/{unit}")]
        public async Task<Incident> GenerateTestUnitUpdate(string unit) {
            var _id = 1;
            var _unit = new AssignedUnit { radioName = unit, statusId = 5 };
            return await this.UpdateIncidentUnits(_id, _unit);
        }

        
    }
}