using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HALALBank.Startup))]
namespace HALALBank
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
