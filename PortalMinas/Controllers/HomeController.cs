using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PortalMinas.helpers;
using PortalMinas.Models;

namespace PortalMinas.Controllers
{
    
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            AuxDataUser du = adAuthenticationServices.GetInformacionUsuario(User);
            EmpresaEntities db = new EmpresaEntities(adAuthenticationServices.GetConnString(du.empresa));

            return View(db.Comp_A.ToList());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}