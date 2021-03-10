using RestSharp;
using RestSharp.Serialization.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace MVC_Project.WebApis.Servicios
{
    public class BaseService
    {
        protected static IRestResponse<T> CallService<T>(string baseUrl,string endpoint,Method method) where T : new()
        {
            RestClient restClient = new RestClient(baseUrl);
            restClient.UseJson();
            RestRequest restRequest = new RestRequest(endpoint, method,DataFormat.Json);
            //restRequest.JsonSerializer = new JsonSerializer();
            return restClient.Execute<T>(restRequest);
        }        

        protected static string AppSettings(string key)
        {
            return WebConfigurationManager.AppSettings[key];
        }
    }
}
