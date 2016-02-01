using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FinalProject1.Startup))]
namespace FinalProject1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
