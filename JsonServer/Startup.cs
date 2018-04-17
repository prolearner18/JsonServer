using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(JsonServer.Startup))]
namespace JsonServer
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
