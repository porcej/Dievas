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

        // // Handle upstream data sources subscribing to push data
        // public async Task JoinDataFeed()
        // {
        //     await Groups.AddToGroupAsync(Context.ConnectionId, "dataFeed");
        // }

        // // Handle upstream data sources disconnecting
        // public async Task LeaveDataFeed()
        // {
        //     await Groups.RemoveFromGroupAsync(Context.ConnectionId, "dataFeed");
        // }
        // // public async Task JoinCADGroup()
    }
}
