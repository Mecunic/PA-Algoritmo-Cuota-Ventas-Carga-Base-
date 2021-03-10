using MVC_Project.WebApis.Modelos;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace MVC_Project.WebApis.Servicios
{
    public class IntermediaService : BaseService
    {
        const string WEB_API = "WEB_API_INTERMEDIA";
        public static List<CedisResp> Cedis()
        {
            var response = CallService<List<CedisResp>>(AppSettings(WEB_API), "/api/Cedis", Method.GET);
            if(response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception(response.Content);
            }
            return response.Data;
        }
    }
}
