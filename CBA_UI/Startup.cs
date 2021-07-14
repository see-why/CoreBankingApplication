using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CBA.Startup))]
namespace CBA
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
