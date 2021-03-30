using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using PortalMinas.helpers;
using PortalMinas.Models;

namespace PortalMinas.Controllers
{
    public class LoginController : Controller
    {

        private readonly minaNubeEntities db = new minaNubeEntities();

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult VerifyData(AuxDataUser datos)
        {
            var ListEmp = db.Usuarios.Where(x => x.NomUsuario.ToLower() == datos.usuario.ToLower() && x.Password == datos.password)
                                        .Select(x => new {
                                            Bloqueado = x.Bloqueado,
                                            ListEmpresa = x.EmpresasUsuarios.Where(y => y.IdUsuario == x.IdUsuario).Select(y => new AuxEmpresa { Id = y.Empresas.IdEmpresa, NomEmpresa = y.Empresas.NombreEmpresa}).ToList()
                                        }).FirstOrDefault();
            
            if(ListEmp == null)               
                return Json("NoUser");
            if(ListEmp.Bloqueado)
                return Json("UserBloq");

            return Json(ListEmp.ListEmpresa);
        }

        [HttpPost]
        [AllowAnonymous]
        public virtual JsonResult Login(AuxDataUser datos)
        {
            Usuarios user = db.Usuarios.Where(x => x.NomUsuario.ToLower() == datos.usuario.ToLower() && x.Password == datos.password).FirstOrDefault();
            
            if (user == null)
                return Json("error: NoUser");
            if (user.Bloqueado)
                return Json("error: UserBloq");

            string rol = user.Roles.NomRol;
            IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
            var authService = new adAuthenticationServices(authenticationManager);
            var authenticationResult = authService.SignIn(datos.usuario, datos.password, datos.empresa, rol);

            if (authenticationResult.IsSuccess)
                return Json("Home/Index");
            else
                return Json("error: NoSignIn");
        }
    }
}