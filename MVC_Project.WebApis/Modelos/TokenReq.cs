using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Project.WebApis.Modelos
{
    public class TokenReq
    {
        public string grant_type { get; set; }
        public string username { get; set; }
        public string password { get; set; }
    }
}
