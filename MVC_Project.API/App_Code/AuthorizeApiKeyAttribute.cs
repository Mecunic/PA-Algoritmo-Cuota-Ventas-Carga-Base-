using MVC_Project.API.Models;
using MVC_Project.API.Models.Api_Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace MVC_Project.API
{
    public class AuthorizeApiKeyAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext filterContext)
        {
            List<MessageResponse> mensajesError = new List<MessageResponse>();
            if (filterContext.Request.Headers.Authorization == null)
            {
                mensajesError.Add(new MessageResponse { Type = "E", Description = "No se encuentra el Api Key" });
                filterContext.Response = filterContext.Request.CreateResponse(HttpStatusCode.Forbidden, new ApiResponse<List<MessageResponse>>()
                {
                    Result = "Error",
                    ResponseData = mensajesError,
                    StatusCode = (int)HttpStatusCode.Forbidden,
                    Message = mensajesError.First().Description
                });
                return;
            }
            string apiKey = filterContext.Request.Headers.Authorization.ToString();
            //using (ISession session = NHibernateHelper.OpenSession())
            //{
            //    UsersBL usersBL = new UsersBL(session);
            //    var usuario = usersBL.GetUsersByApikey(apiKey);
            //    if (usuario == null)
            //    {
            //        mensajesError.Add(new Message { Tipo = "E", Mensaje = Resources.MENSAJES_API.ERROR_HEADER_AUTH_INVALID });
            //        filterContext.Response = filterContext.Request.CreateResponse(HttpStatusCode.Forbidden, new ApiResponse<List<Message>>()
            //        {
            //            Result = "Error",
            //            ResponseData = mensajesError,
            //            StatusCode = (int)HttpStatusCode.Forbidden,
            //            ErrorMessage = mensajesError.First().Mensaje
            //        });
            //        return;
            //    }
            //    DateTime dateNow = DateUtil.GetDateTimeNow();
            //    if (string.IsNullOrEmpty(usuario.ApiKeyExpiration) || DateTime.ParseExact(usuario.ApiKeyExpiration, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture) < dateNow)
            //    {
            //        mensajesError.Add(new Message { Tipo = "E", Mensaje = Resources.MENSAJES_API.ERROR_HEADER_AUTH_INVALID });
            //        filterContext.Response = filterContext.Request.CreateResponse(HttpStatusCode.Forbidden, new ApiResponse<List<Message>>()
            //        {
            //            Result = "Error",
            //            ResponseData = mensajesError,
            //            StatusCode = (int)HttpStatusCode.Forbidden,
            //            ErrorMessage = mensajesError.First().Mensaje
            //        });
            //        return;
            //    }
            //}
        }
    }
}