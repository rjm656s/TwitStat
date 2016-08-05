using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TwitStat.Startup))]
namespace TwitStat
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
