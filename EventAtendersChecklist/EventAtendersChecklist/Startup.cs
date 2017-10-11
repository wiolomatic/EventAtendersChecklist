using Microsoft.Owin;

[assembly: OwinStartupAttribute(typeof(EventAtendersChecklist.Startup))]
namespace EventAtendersChecklist
{
    using Owin;

    /// <summary>
    /// Defines the <see cref="Startup" />
    /// </summary>
    public partial class Startup
    {
        /// <summary>
        /// The Configuration
        /// </summary>
        /// <param name="app">The <see cref="IAppBuilder"/></param>
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
            ConfigureAuth(app);
        }
    }
}
