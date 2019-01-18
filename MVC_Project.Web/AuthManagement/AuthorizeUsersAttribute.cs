using MVC_Project.Web.AuthManagement.Models;
using System;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MVC_Project.Web.AuthManagement
{
    [AttributeUsageAttribute(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class AuthorizeUsersAttribute : AuthorizeAttribute
    {                
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (Authenticator.AuthenticatedUser != null && filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new System.Web.Mvc.HttpStatusCodeResult((int)System.Net.HttpStatusCode.Forbidden);
            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }            
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {                                 
            AuthUser authenticatedUser = Authenticator.AuthenticatedUser;
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