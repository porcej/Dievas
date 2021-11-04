using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Models;
using Microsoft.AspNetCore.SignalR;

namespace Backend.Hubs {

    public interface IDashboardHub {
        Task IncidentAdded(Incident incident);
        Task IncidentFieldChanged(int incidentId, string field, string value);
        Task IncidentUnitStatusChanged(int incidentId, AssignedUnit unit);
        Task IncidentCommentAdded(int incidentId, Comment comment);
        Task UnitStatusChanged(string radioName, int statusId);
        Task UnitHomeChanged(string radioName, string homeStation);
    }

    //  Here we handle general client communications
    public class DashboardHub: Hub<IDashboardHub> {
        
        private static CAD _cad;

        public DashboardHub(CAD cad){
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
        }

        // Handle upstream data sources disconnecting
        public async Task LeaveDataFeed() {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "dataFeed");
        }
        // public async Task JoinCADGroup()

        // ***************************************************************************************\
        // ***************************************************************************************/
        // Please update below to handle incoming data
        // * The Dashboard clients accept the following actions sent to the "dashboard" group:
        //      * IncidentAdded(incident)
        //          - incident is of class Backend.Models.Incident

        // * The Dashboarc clients accept the following actions sent to the group whos name is
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
            Incident incident = new Incident {
                id = incidentId,
                Units = new List<AssignedUnit>{ new AssignedUnit { radioName = radioName, statusId = 1 } }
            };

            await Clients.Group("dashboard").IncidentAdded(incident);
            _cad.AddIncident(incident);
        }
        public async Task UpdateIncidentType(int incidentId, string newIncidentType){
            await Clients.Group(incidentId.ToString())
                   .IncidentFieldChanged(incidentId, "incidentType", newIncidentType);

            _cad.UpdateIncidentField(incidentId, "incidentType", newIncidentType);

        }
    }
}
