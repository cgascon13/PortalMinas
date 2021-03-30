using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PortalMinas.Models;
using System.Security.Claims;
using PortalMinas.App_Start;
using System.Security.Principal;
using System.Configuration;
using System.Web.Configuration;

namespace PortalMinas.helpers
{
    public class adAuthenticationServices
    {
        private readonly minaNubeEntities db = new minaNubeEntities();


        public class AuthenticationResult
        {
            public AuthenticationResult(string errorMessage = null)
            {
                ErrorMessage = errorMessage;
            }

            public String ErrorMessage { get; private set; }
            public Boolean IsSuccess => String.IsNullOrEmpty(ErrorMessage);
        }

        private readonly IAuthenticationManager authenticationManager;

        public adAuthenticationServices(IAuthenticationManager authenticationManager)
        {
            this.authenticationManager = authenticationManager;
        }

        public AuthenticationResult SignIn(String username, String password, String empresa, String rol)
        {
            try
            {
               
                    Models.Usuarios u = db.Usuarios.Where(x => x.NomUsuario == username).FirstOrDefault();
                    if (u != null)
                    {
                        if (u.Password.Equals(password))
                        {
                            // SessionExtensions.Set<Models.Usuarios>(HttpContext.Session, "_Usuario", u);
                            HttpContext.Current.Session["_Usuario"] = u;
                            var identity = CreateIdentity(username, rol, empresa);
                            authenticationManager.SignOut(PortalMinasAuthentication.ApplicationCookie);
                            authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, identity);
                            return new AuthenticationResult();
                        }
                        return new AuthenticationResult("0");
                    }
                    return new AuthenticationResult("0");
                
            }
            catch (Exception ex)
            {
                string val = "Acceso Invalido No se pudo crear su usuario 105: " + ex.Message;
                return new AuthenticationResult(val);
            }
        }

        private ClaimsIdentity CreateIdentity(string userPrincipal, string rol, string empresa)
        {
            //GestorOWS.Service1Client gestor = new GestorOWS.Service1Client();
            //GestorOWS.Usuarios u = gestor.getUsuario(userPrincipal);
            //string rol = gestor.getRol((int)u.IDRol);

            var identity = new ClaimsIdentity(PortalMinasAuthentication.ApplicationCookie, ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            identity.AddClaim(new Claim(ClaimTypes.Name, userPrincipal));
            identity.AddClaim(new Claim(ClaimTypes.Role, rol));
            identity.AddClaim(new Claim("Empresa", empresa));
            identity.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", userPrincipal));
            identity.AddClaim(new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", userPrincipal));

            // add your own claims if you need to add more information stored on the cookie

            return identity;
        }

        public static AuxDataUser GetInformacionUsuario(IPrincipal User)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return null;
            }
            AuxDataUser du = new AuxDataUser();

            du.usuario = ((ClaimsPrincipal)User).Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;
            du.rol = ((ClaimsPrincipal)User).Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
            du.empresa = ((ClaimsPrincipal)User).Claims.FirstOrDefault(c => c.Type == "Empresa").Value;

            return du;
        }

        public static string GetConnString(string db)
        {
            System.Configuration.Configuration Config1 = WebConfigurationManager.OpenWebConfiguration("~");
            ConnectionStringsSection conSetting = (ConnectionStringsSection)Config1.GetSection("connectionStrings");
            //conSetting.ConnectionStrings["EmpresaEntities"].ConnectionString = conSetting.ConnectionStrings["EmpresaEntities"].ConnectionString.ToString().Replace(Properties.Settings.Default.BdPrincipal, db);
            string newConn = conSetting.ConnectionStrings["EmpresaEntities"].ConnectionString.ToString().Replace(Properties.Settings.Default.BdPrincipal, db);

            return newConn;
        }
    }
}