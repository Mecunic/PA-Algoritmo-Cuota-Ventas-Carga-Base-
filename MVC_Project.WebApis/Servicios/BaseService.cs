using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Serialization.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
            return restClient.Execute<T>(restRequest);
        }

        protected static IRestResponse<T> CallService<T>(string baseUrl, string endpoint, Method method, Dictionary<string, object> parametros) where T : new()
        {
            RestClient restClient = new RestClient(baseUrl);
            restClient.UseJson();
            RestRequest restRequest = new RestRequest(endpoint, method, DataFormat.Json);
            if (parametros.Any())
            {
                foreach (KeyValuePair<string, object> entry in parametros)
                {
                    restRequest.AddParameter(entry.Key, entry.Value);
                }
            }
            return restClient.Execute<T>(restRequest);
        }

        protected static IRestResponse<T> CallService<T>(string baseUrl, string endpoint, Method method,object body) where T : new()
        {
            RestClient restClient = new RestClient(baseUrl);
            restClient.UseJson();
            RestRequest restRequest = new RestRequest(endpoint, method, DataFormat.Json);
            restRequest.AddJsonBody(body);
            return restClient.Execute<T>(restRequest);
        }

        protected static IRestResponse<T> CallServiceUrlEncoder<T,V>(string baseUrl, string endpoint, Method method, V body) where T : new()
        {
            RestClient restClient = new RestClient(baseUrl);
            
            RestRequest restRequest = new RestRequest(endpoint, method, DataFormat.Json);
            restRequest.AddHeader("content-type", "application/x-www-form-urlencoded");
            
            string parametros = "";
            foreach (PropertyInfo prop in body.GetType().GetProperties())
            {
                parametros += string.Format("{0}={1}&",prop.Name,prop.GetValue(body)); 
            }
            restRequest.AddParameter("application/x-www-form-urlencoded", parametros, ParameterType.RequestBody);
            return restClient.Execute<T>(restRequest);
        }

        protected static string AppSettings(string key)
        {
            return WebConfigurationManager.AppSettings[key];
        }
    }
}
