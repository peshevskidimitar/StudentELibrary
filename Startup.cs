using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(StudentELibrary.Startup))]
namespace StudentELibrary
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
