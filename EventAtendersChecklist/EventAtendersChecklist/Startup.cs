using Microsoft.Owin;
using Owin;
using EventAtendersChecklist;

[assembly: OwinStartupAttribute(typeof(EventAtendersChecklist.Startup))]
namespace EventAtendersChecklist
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
            ConfigureAuth(app);
        }
    }
}
