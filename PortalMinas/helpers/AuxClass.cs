using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalMinas.helpers
{
    public class AuxDataUser
    {
        public string usuario { get; set; }
        public string password { get; set; }
        public string empresa { get; set; }
        public string rol { get; set; }
    }

    public class AuxUsuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public bool Bloqueado { get; set; }
        public string Pass { get; set; }
        public List<AuxEmpresa> ListEmpresa { get; set; }
        public string Rol { get; set; }
    }

    public class AuxEmpresa
    {
        public int Id { get; set; }
        public string NomEmpresa { get; set; }
    }
}