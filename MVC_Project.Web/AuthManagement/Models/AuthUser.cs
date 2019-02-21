using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_Project.Web.AuthManagement.Models
{
    public class AuthUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
        public IList<Permission> Permissions { get; set; }

        public bool HasAccessSection(string section)
        {
            if (this.Permissions != null && this.Permissions.Count > 0)
            {
                var per = from ap in this.Permissions where ap.Action == section select ap;
                return per.Count<Permission>() > 0;
            }
            return false;
        }
    }

    public class Role
    {
        public string Code { get; set; }        
    }

    public class Permission
    {
        public string Controller { get; set; }
        public string Action { get; set; }
    }
    
}