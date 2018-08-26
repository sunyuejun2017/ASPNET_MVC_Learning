using Microsoft.Owin;
using Owin;

using Microsoft.Owin.Security.ActiveDirectory;
using System.Configuration;

[assembly:OwinStartup(typeof(CH5_WEBAPI.Startup))]
namespace CH5_WEBAPI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
