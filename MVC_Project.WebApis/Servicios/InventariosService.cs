﻿using MVC_Project.WebApis.Modelos;
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

        /*private static T Execute<T>(RestRequest request) where T : new()
        {
            IRestClient client = new RestClient(WebConfigurationManager.AppSettings[WEB_API]);
            var response = client.Execute<T>(request);
            if (!response.IsSuccessful)
            {
                throw new Exception(response.Content);
            }
            if (response.ErrorException != null)
            {
                throw new Exception("error", response.ErrorException);
            }
            return response.Data;
        }*/

        public static LoginResp Login(string username, string password, int opcd)
        {
            //var request = new RestRequest($"{USERS_API}/login", Method.POST);
            var param = new LoginReq()
            {
                Credentials = new Credentials()
                {
                    Username = username,
                    Password = password,
                    Opcd = opcd
                }
            };

            //request.AddJsonBody(param);
            //return Execute<LoginResp>(request);
            var response = CallService<LoginResp>(AppSettings(WEB_API), $"{USERS_API}/login", Method.POST, param);
            return response.Data;

        }

        public static List<UsuariosResp> Usuarios(int CedisId)
        {
            var parametros = new Dictionary<string, object>() { { "salepoint",CedisId } };
            var response = CallService<List<UsuariosResp>>(AppSettings(WEB_API), $"{USERS_API}/users", Method.GET, parametros);
            return response.Data;
        }


    }
}
