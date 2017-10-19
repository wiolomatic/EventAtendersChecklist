using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace EventAtendersChecklist.SignalR
{
    [HubName("signalRHub")]
    public class SignalRHub : Hub
    {
        [HubMethodName("notifyChanges")]
        public static void NotifyChanges()
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<SignalRHub>();
                
            context.Clients.All.notifyChanges();
        }
    }
}