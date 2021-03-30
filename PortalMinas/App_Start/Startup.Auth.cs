using System;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

namespace PortalMinas.App_Start
{
    public static class PortalMinasAuthentication
    {
        public const String ApplicationCookie = "PortalMinasAuthenticationType";
    }

    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            // need to add UserManager into owin, because this is used in cookie invalidation
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = PortalMinasAuthentication.ApplicationCookie,
                LoginPath = new PathString("/Login/Login"),
                Provider = new CookieAuthenticationProvider(),
                CookieName = "PortalMinasCookie",
                CookieHttpOnly = true,
                ExpireTimeSpan = TimeSpan.FromHours(1), // adjust to your needs
            });
        }
    }
}