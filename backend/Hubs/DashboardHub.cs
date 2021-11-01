using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Hubs
{
  public class DashboardHub : Hub
  {

    public async Task Subscribe(string groupName)
    {
      await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
    }

    public async Task Unsubscribe(string groupName)
    {
      await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
    }

    //aliases for Subscribe methods
    public async Task JoinIncidentGroup(string incidentId)
    {
      await Subscribe(incidentId);
    }

    public async Task LeaveIncidentGroup(string incidentId)
    {
      await Unsubscribe(incidentId);
    }
    public async Task SendMessage(string sender, string message)
    {
      await Clients.All.SendAsync("ReceiveMessage", sender, message);
    }
    public async Task SendMessageToGroup(string groupName, string sender, string message)
    {
      await SendActionToGroup("ReceiveGroupMessage", groupName, sender, message);
    }
    public async Task SendActionToGroup(string action, string groupName, string sender, string message)
    {
      switch (action)
      {
        case "IncidentAdded":
          await Clients.Group(groupName).SendAsync("IncidentAdded", message);
          break;
        default:
          await Clients.Group(groupName).SendAsync(action, groupName, sender, message);
          break;
      }

    }
  }
}
