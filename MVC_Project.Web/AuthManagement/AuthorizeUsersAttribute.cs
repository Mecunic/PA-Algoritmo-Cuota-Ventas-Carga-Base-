using MVC_Project.Web.AuthManagement.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Project.Web.AuthManagement
{
    [AttributeUsageAttribute(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class AuthorizeUsersAttribute : AuthorizeAttribute
    {                
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new { controller = "Auth", action = "Login" }));
            }
            else
            {
                filterContext.Result = new System.Web.Mvc.HttpStatusCodeResult((int)System.Net.HttpStatusCode.Forbidden);
            }
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            AuthUser authenticatedUser = new AuthUser{
                Role = new Role
                {
                    Code = "ADMIN"
                }
            }; // TODO retrieve the authenticated user                        
            if (authenticatedUser != null)
            {
                string controller = httpContext.Request.RequestContext.RouteData.Values["controller"].ToString();
                string action = httpContext.Request.RequestContext.RouteData.Values["action"].ToString();
                if (authenticatedUser.Role.Code.Equals(ConfigurationManager.AppSettings.Get("AdminKey")))
                {
                    return true;
                }
                if (authenticatedUser.Permissions.Any(permission => permission.Controller.Equals(controller) && permission.Action.Equals(action)))
                {
                    return true;
                }
                return false;
            }
            return false;
        }
    }
}