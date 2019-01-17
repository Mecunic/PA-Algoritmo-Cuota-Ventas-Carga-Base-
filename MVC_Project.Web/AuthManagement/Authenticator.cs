﻿using MVC_Project.Web.AuthManagement.Models;
using System.Web.Security;

namespace MVC_Project.Web.AuthManagement
{
    public class Authenticator
    {
        public static AuthUser AuthenticatedUser
        {
            get
            {
                var authUser = System.Web.HttpContext.Current.Session["ST_AUTH_USER"];
                return authUser != null ? (AuthUser)authUser : null;
            }
        }
        public static void StoreAuthenticatedUser(AuthUser authUser)
        {
            System.Web.HttpContext.Current.Session.Add("ST_AUTH_USER", authUser);
            FormsAuthentication.SetAuthCookie(authUser.Email, true);
        }

        public static void RemoveAuthenticatedUser()
        {            
            System.Web.HttpContext.Current.Session.Remove("ST_AUTH_USER");            
            FormsAuthentication.SignOut(); 
          
        }
    }
}