using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using AFD.Dashboard.Models;

namespace Dievas.Hubs {

    public interface IDashboardHub {
        Task IncidentAdded(IncidentDto incident);
        Task IncidentFieldChanged(int incidentId, string field, Object value);
        Task IncidentUnitStatusChanged(int incidentId, UnitAssignmentDto unit);
        Task IncidentCommentAdded(int incidentId, CommentDto comment);
        Task UnitStatusChanged(string radioName, int statusId);
        Task UnitFieldChanged(string radioName, string field, Object value);
        Task GetAllIncidents(int minutesPast);
        Task GetAllUnits();
    }

    //  Here we handle general client communications
    public class DashboardHub: Hub<IDashboardHub> {
        
        private static CAD _cad;

        private readonly IConfiguration _config;

        public DashboardHub(IConfiguration configuration, CAD cad){
            _config = configuration;
            _cad = cad;
        }

        // Handle clients connecting
        public async Task JoinDashboard() {
            await Groups.AddToGroupAsync(Context.ConnectionId, "dashboard");
        }

        // Handle clients disconnecting
        public async Task LeaveDashboard() {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId,
                                              "dashboard");
        }

        // Add clients to an incident 
        public async Task JoinIncidentGroup(int incidentId){
            await Groups.AddToGroupAsync(Context.ConnectionId,
                                         incidentId.ToString());
        }

        // Remove clients from an incident
        public async Task LeaveIncidentGroup(int incidentId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, 
                                              incidentId.ToString());
        }


        // ***************************************************************************************\
        // ***************************************************************************************/
        // Please have datafeeds run JoinDataFeed() on connection 
        //      and LeaveDataFeed() on disconnect.  Also please seed all message

        // Handle upstream data sources subscribing to push data
        public async Task JoinDataFeed() {
            await Groups.AddToGroupAsync(Context.ConnectionId, "dataFeed");
            await GetAllIncidents(4320);
            await Clients.Group("dataFeed").GetAllUnits();
        }

        // Handle upstream data sources disconnecting
        public async Task LeaveDataFeed() {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "dataFeed");
        }

        // ***************************************************************************************\
        // ***************************************************************************************/
        // Methods employed by datafeeds to send data out

        // Receive all incidents on initialization
        public async Task AllIncidents(IEnumerable<IncidentDto> incidents)
        {
            foreach (var incident in incidents)
            {
                _cad.AddIncident(incident);
                await Clients.Group("dashboard").IncidentAdded(incident);
            }
        }

        //Receive all units on initialization
        public async Task AllUnits(IEnumerable<UnitDto> units)
        {
            _cad.PopulateUnitList(units);
        }

        // Add new incident
        public async Task IncidentAdded(IncidentDto incident) {
            await Clients.Group("dashboard").IncidentAdded(incident);
            _cad.AddIncident(incident);
            double hoursToKeep = 0;
            Double.TryParse(_config["Hub:HoursToKeepIncidents"], out hoursToKeep);
            _cad.RemoveClosedIncidents(hoursToKeep);
        }

        // Update incident field
        public async Task IncidentFieldChanged(int incidentId, string field, string value){
            await Clients.Group("dashboard").IncidentFieldChanged(incidentId, field, value);
            _cad.UpdateIncidentField(incidentId, field, value);
        }

        // Change unit status
        public async Task IncidentUnitStatusChanged(int incidentId, UnitAssignmentDto unit){
            await Clients.Group("dashboard").IncidentUnitStatusChanged(incidentId, unit);
            _cad.AddOrUpdateIncidentUnit(incidentId, unit);
        }

        // Add comment to incidnet
        public async Task IncidentCommentAdded(int incidentId, CommentDto comment) {
            await Clients.Group("dashboard").IncidentCommentAdded(incidentId, comment);
            _cad.AddOrUpdateIncidentComment(incidentId, comment);
        }

        // Update unit status for non-incidents
        public async Task UnitStatusChanged(string radioName, int statusId) {
            await Clients.Group("dashboard").UnitStatusChanged(radioName, statusId);
            _cad.UpdateUnitStatus(radioName, statusId);
        }

        public async Task UnitFieldChanged(string radioName, string field, string value)
        {
            await Clients.Group("dashboard").UnitFieldChanged(radioName, field, value);
            _cad.UpdateUnitField(radioName, field, value);
        }

        // ***************************************************************************************\
        // ***************************************************************************************/
        // Please update below to handle incoming data
        // * The Dashboard clients accept the following actions sent to the "dashboard" group:
        //      * IncidentAdded(incident)
        //          - incident is of class Backend.Models.Incident

        // * The Dashboard clients accept the following actions sent to the group whos name is
        //   the string representation of the incident id (incidentId.ToString()) 
        //      * IncidentFieldChanged(incidentId, fieldName, value)
        //          - incidentId is an integer id for the incident (provided upstream)
        //          - fieldName is a string representation of the updated's field 
        //             can be any field name in Backend.Models.Incident except units or comments
        //          - value is a string representation of the new value
        //      * IncidentUnitStatusChanged(incidentId, unit)
        //          - incidentId is an integer id for the incident (provided upstream)
        //          - unit is of class Backend.Models.AssignedIncident
        //      * IncidentCommentAdded(incidentId, comment)
        //          - incidentId is an integer id for the incident (provided upstream)
        //          - comment if of class Backend.Models.Comment

        // The following are example hooks

        public async Task AddNewIncidentWithOneUnitInTheDispatchedStatus(int incidentId, string radioName){
            IncidentDto incident = new IncidentDto
            {
                Id = incidentId,
                UnitsAssigned = new List<UnitAssignmentDto>{ new UnitAssignmentDto { RadioName = radioName, StatusId = 1 } }
            };

            await Clients.Group("dashboard").IncidentAdded(incident);
            _cad.AddIncident(incident);
        }
        public async Task UpdateIncidentType(int incidentId, string newIncidentType){
            await Clients.Group(incidentId.ToString())
                   .IncidentFieldChanged(incidentId, "incidentType", newIncidentType);

            _cad.UpdateIncidentField(incidentId, "incidentType", newIncidentType);

        }

        public async Task GetAllIncidents(int minutesPast)
        {
            await Clients.Group("dataFeed").GetAllIncidents(minutesPast);
        }
    }
}
