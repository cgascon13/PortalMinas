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
        //private readonly AuxDataUser du2 = adAuthenticationServices.GetInformacionUsuario(User);
        public ActionResult Index()
        {
            AuxDataUser du2 = adAuthenticationServices.GetInformacionUsuario(User);
            EmpresaEntities db2 = new EmpresaEntities(adAuthenticationServices.GetConnString(du2.empresa));

            return View(db2.Comp_A.ToList());
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