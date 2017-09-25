using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EventAtendersChecklist.Startup))]
namespace EventAtendersChecklist
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
