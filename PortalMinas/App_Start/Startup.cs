using Microsoft.Owin;
using Owin;
using System;
using System.Web.Mvc;

[assembly: OwinStartupAttribute(typeof(PortalMinas.App_Start.Startup))]
namespace PortalMinas.App_Start
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            ViewEngines.Engines.Clear();
            IViewEngine razorEngine = new RazorViewEngine() { FileExtensions = new string[] { "cshtml" } };
            ViewEngines.Engines.Add(razorEngine);
        }

        
    }
}