using Microsoft.Owin.Security.Cookies;
using Owin;

namespace QuickbooksWeb
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions {
                AuthenticationType = "Cookies"
            });
        }
    }
}