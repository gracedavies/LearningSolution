using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ClientWebsite.Startup))]
namespace ClientWebsite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
