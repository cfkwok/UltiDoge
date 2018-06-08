using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(UltiDogeWebServer.Startup))]
namespace UltiDogeWebServer
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
