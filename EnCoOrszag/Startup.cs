using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EnCoOrszag.Startup))]
namespace EnCoOrszag
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
