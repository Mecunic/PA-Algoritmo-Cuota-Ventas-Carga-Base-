using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Project.WebApis.Modelos
{
    public class LoginResp
    {
        public int ResultCode { get; set; }
        public int ErrorMsg { get; set; }
        public Result Result { get; set; }
    }

    public class Result
    {
        public string Token { get; set; }
        public string Opcd { get; set; }
        public string Username { get; set; }
        public int Expiration { get; set; }
    }
}
