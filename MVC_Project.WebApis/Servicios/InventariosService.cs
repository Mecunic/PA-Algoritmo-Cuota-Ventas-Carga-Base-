using MVC_Project.WebApis.Modelos;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Web.Configuration;

namespace MVC_Project.WebApis.Servicios
{
    public class InventariosService : BaseService
    {
        const string WEB_API = "WEB_API_INVENTARIOS";
        const string USERS_API = "/users/api";

        public static LoginResp Login(string username, string password, int opcd)
        {
            var param = new LoginReq()
            {
                Credentials = new Credentials()
                {
                    Username = username,
                    Password = password,
                    Opcd = opcd
                }
            };
            var response = CallService<LoginResp>(AppSettings(WEB_API), $"{USERS_API}/login", Method.POST, param);
            return response.Data;

        }

        public static List<UsuariosResp> Usuarios(int CedisId)
        {
            var parametros = new Dictionary<string, object>() { { "salepoint", CedisId } };
            var response = CallService<List<UsuariosResp>>(AppSettings(WEB_API), $"{USERS_API}/users", Method.GET, parametros);
            return response.Data;
        }

    }
}
