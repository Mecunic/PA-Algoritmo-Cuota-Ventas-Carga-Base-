using MVC_Project.API.Models;
using MVC_Project.API.Models.Api_Response;
using MVC_Project.Data.Helpers;
using MVC_Project.Domain.Entities;
using MVC_Project.Domain.Helpers;
using MVC_Project.Domain.Services;
using MVC_Project.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Unity;

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
            using (IUnitOfWork unitOfWork = new UnitOfWork())
            {
                IUserService userService = UnityConfig.Container.Resolve<IUserService>();
                User user = userService.FindBy(x => x.ApiKey == apiKey).FirstOrDefault();
                DateTime now = DateUtil.GetDateTimeNow();
                if (user == null || (user.ExpiraApiKey.HasValue && user.ExpiraApiKey.Value < now))
                {
                    filterContext.Response = filterContext.Request.CreateResponse(HttpStatusCode.Unauthorized, new { message = "Invalid authorization token" });
                    return;
                }
                filterContext.RequestContext.Principal = new GenericPrincipal(new GenericIdentity(user.Id.ToString()), null);
            }
        }
    }
}