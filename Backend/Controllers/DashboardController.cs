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

        private static ConcurrentDictionary<int, Incident> _incidents = new ConcurrentDictionary<int, Incident>();

        //     {9, 
        //       new Incident {
        //             id = 0,
        //             active = true,
        //             jurisdiction = "200 ALX",
        //             incidentType = "FIRE-LOCAL ALARM",
        //             LocationName = "ALEXANDRIA FIRE STATION 204",
        //             address = "900 2ND St",
        //             apartment = "",
        //             city = "CITY OF ALEXANDRIA",
        //             state = "VA",
        //             postalCode = "22314",
        //             county = "Alexandria",
        //             locationType = "Government or Public Building",
        //             longitude = -77.0467847,
        //             latitude = 38.8163701,
        //             crossStreet = "POWHATAN ST",
        //             commandChannel = "",
        //             primaryTACChannel = "2 BRAVO",
        //             alternateTACChannel = "",
        //             callDisposition = "",
        //             incidentStartTime = DateTime.Now,
        //             // incidentEndTime = "",
        //             Comments = new List<Comment>{ new Comment { id = 0, commentText = "incident notes here"}},
        //             Units = new List<AssignedUnit>{ new AssignedUnit { radioName = "E204", statusId = 1 } }
        //         }
        // };

        public DashboardController(IHubContext<DashboardHub, IDashboardHub> dashboardHub,
                                   IConfiguration configuration,
                                   ILogger<DashboardController> logger) {

            _hubContext = dashboardHub;
            _config = configuration;
            _logger = logger;
        }

        // This is bad form for a strongly typed language, we should replace this
        // with direct access to each field
        private Incident UpdateIncidentField(Incident incident, string field, string value){
            switch (field) {
                case "active":
                    incident.active = (value.ToLower() == "true");
                    break;
                case "jurisdiction":
                    incident.jurisdiction = value;
                    break;
                case "incidentType":
                    incident.incidentType = value;
                    break; 
                case "LocationName":
                    incident.LocationName = value;
                    break; 
                case "address":
                    incident.address = value;
                    break; 
                case "apartment":
                    incident.apartment = value;
                    break; 
                case "city":
                    incident.city = value;
                    break; 
                case "state":
                    incident.state = value;
                    break; 
                case "postalCode":
                    incident.postalCode = value;
                    break; 
                case "county":
                    incident.county = value;
                    break; 
                case "locationType":
                    incident.locationType = value;
                    break; 
                case "crossStreet":
                    incident.crossStreet = value;
                    break; 
                case "commandChannel":
                    incident.commandChannel = value;
                    break; 
                case "primaryTACChannel":
                    incident.primaryTACChannel = value;
                    break; 
                case "alternateTACChannel":
                    incident.alternateTACChannel = value;
                    break; 
                case "callDisposition":
                    incident.callDisposition = value;
                    break;
                case "longitude":
                    incident.longitude = Convert.ToDouble(value);
                    break;
                case "latitude":
                    incident.latitude = Convert.ToDouble(value);
                    break;
                case "incidentStartTime":
                    incident.incidentStartTime = DateTime.Parse(value);
                    break;
                case "incidentEndTime":
                    incident.incidentEndTime = DateTime.Parse(value);
                    break;
            }
            return incident;
        }

        // Readonly web API for incidents information
        [HttpGet("incidents")]
        public IEnumerable GetIncidents() {
            // Bags are unordered by default, so we have to apply order
            return _incidents.Values.OrderBy( t => t.id );
        }

        [HttpGet("incidents/active/")]
        public IEnumerable GetActiveIncidents() {
            return _incidents.Values.Where(t => t.active).OrderBy( t => t.id );
        }

        [HttpGet("incident/{id}")]
        public ActionResult GetIncident(int id) {            
            if (_incidents.ContainsKey(id)) return NotFound();
            var incident = _incidents[id];
            return new JsonResult(incident);
        }

        // Web API for data feeds to add incident information

        // Add (or updates) new incident
        [HttpPost("feed/incident")]
        [Authorize]
        public async Task<Incident> AddIncident([FromBody]Incident incident) {
            int _key = incident.id;
            _incidents[_key] = incident;
            await this._hubContext.Clients.Group("dashboard").IncidentAdded(incident);
            return incident;
        }

        // Add existing incident by id
        [HttpPost("feed/incident/{id}")]
        [Authorize]
        public async Task<Incident> AddIncidentById(int id, [FromBody]Incident incident) {
             _incidents[id] = incident;
            await this._hubContext.Clients.Group("dashboard").IncidentAdded(incident);
            return incident;
        }

        // // Update existing incident
        [HttpPost("feed/incident/{id}/update")]
        [Authorize]
        public async Task<Incident> UpdateIncident(int id, string field, string value) {

            if (!_incidents.ContainsKey(id)) return new Incident {};

            _incidents[id] = UpdateIncidentField(_incidents[id], field, value);
            await this._hubContext
                      .Clients
                      .Group(id.ToString())
                      .IncidentFieldChanged(id, field, value);
            return _incidents[id];
            
        }

        // Add or update unit on incident
        [HttpPost("feed/incident/{id}/unit")]
        [Authorize]
        public async Task<Incident> UpdateIncidentUnits(int id, [FromBody]AssignedUnit unit) {

            if (!_incidents.ContainsKey(id)) return new Incident {};

            var _incident = _incidents[id];
            var _unitKey = _incident.Units.IndexOf(unit);
            
            if (_unitKey < 0) {
                _incident.Units.Add(unit);
            } else {
                _incident.Units[_unitKey] = unit;
            }
            _incidents[id] = _incident;
            await this._hubContext
                        .Clients
                        .Group(id.ToString())
                        .IncidentUnitStatusChanged(id, unit);

            return _incident;
        }

        // [HttpPost("feed/incident/{id}/unit")]
        // [Authorize]
        // public async Task<Incident> UpdateIncidentUnit(int id, string radioName, int statusId) {

        //     if (!_incidents.ContainsKey(id)) return new Incident {};

        //     var _unit = new AssignedUnit { 
        //         radioName = radioName,
        //         statusId = statusId
        //     };

        //     var _incident = _incidents[id];
        //     var _unitKey = _incident.Units.IndexOf(_unit);
        //     if (_unitKey > 0) {
        //         _incident.Units[_unitKey] = _unit;
        //     } else {
        //         _incident.Units.Add(_unit);
        //     }
        //     _incidents[id] = _incident;
        //     await this._hubContext
        //                 .Clients
        //                 .Group(id.ToString())
        //                 .IncidentUnitStatusChanged(id, _unit);

        //     return _incident;
        // }

        [HttpPost("feed/incident/{id}/comment")]
        [Authorize]
        public async Task<Incident> AddIncidentComment(int id, Comment comment) {

            if (!_incidents.ContainsKey(id)) return new Incident {};


            var _incident = _incidents[id];
            _incident.Comments.Add(comment);
            
            _incidents[id] = _incident;
            await this._hubContext
                        .Clients
                        .Group(id.ToString())
                        .IncidentCommentAdded(id, comment);

            return _incident;
        }

        // TEST FUNCTIONS PLEASE REMOVE
        [HttpGet("test")]
        public async Task<Incident> GenerateTestIncident() {
            var _nextCount = _incidents.Count + 1;
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