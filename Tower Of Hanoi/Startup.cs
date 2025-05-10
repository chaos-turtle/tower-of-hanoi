using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Tower_Of_Hanoi.Startup))]
namespace Tower_Of_Hanoi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
