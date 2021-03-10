using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Project.WebApis.Servicios
{
    public class BaseService
    {
        public static IRestResponse<T> CallService<T>(string baseUrl,string endpoint,Method method) where T : new()
        {
            RestClient restClient = new RestClient(baseUrl);
            RestRequest restRequest = new RestRequest(endpoint, method);
            return restClient.Execute<T>(restRequest);
        }
    }
}
