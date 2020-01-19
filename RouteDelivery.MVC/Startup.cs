using Hangfire;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RouteDelivery.Startup))]
namespace RouteDelivery
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            GlobalConfiguration.Configuration.UseSqlServerStorage("HangfireR3");

            app.UseHangfireDashboard();
            app.UseHangfireServer();

            DIConfig.Setup();
        }
    }
}
