using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TheRustyCauldron_PFG.Startup))]
namespace TheRustyCauldron_PFG
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
