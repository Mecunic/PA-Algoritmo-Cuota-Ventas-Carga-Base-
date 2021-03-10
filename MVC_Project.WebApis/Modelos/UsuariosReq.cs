using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Project.WebApis.Modelos
{
    public class UsuariosResp
    {
        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string name { get; set; }
        public int opcd { get; set; }
    }
}
