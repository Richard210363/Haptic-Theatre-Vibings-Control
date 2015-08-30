using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Haptic_Theatre_Vibings_Control.Startup))]
namespace Haptic_Theatre_Vibings_Control
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
