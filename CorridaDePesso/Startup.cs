using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CorridaDePesso.Startup))]
namespace CorridaDePesso
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
