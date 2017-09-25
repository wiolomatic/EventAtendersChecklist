using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace EventAtendersChecklist.Hub
{
    public class EmployeesHub : Microsoft.AspNet.SignalR.Hub
    {
        private static string conString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();

        public void Hello()
        {
            Clients.All.hello();
        }

        [HubMethodName("sendEmployees")]
        public static void SendEmployees()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<EmployeesHub>();
            context.Clients.All.updateEmployees();
        }
    }
}