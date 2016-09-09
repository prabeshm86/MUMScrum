using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MUMScrum.Web.Startup))]
namespace MUMScrum.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
