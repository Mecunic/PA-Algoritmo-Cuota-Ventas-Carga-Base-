using MVC_Project.Web.CustomAttributes.Validations;
using MVC_Project.WebBackend.Models.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Mvc;

namespace MVC_Project.WebBackend.Models
{
    public class DataTableUsersModel
    {
        public int recordsFiltered { get; set; }
        public int recordsTotal { get; set; }

        [DataMember(Name = "data")]
        public IList<UserData> Data { get; set; }
    }

    public class UserViewModel : DataTableModel
    {
        [Display(Name = "Nombre")]
        public string Name { get; set; }
        public UserData UserList { get; set; }
    }

    public class UserData
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Uuid { get; set; }

        public string UserName { get; set; }
        public bool Status { get; set; }

        //Attributes Custom
        public string CedisName { get; set; }
    }

    public class UserSaveViewModel
    {
        [Required]
        [ValidType(CustomAttributes.Validations.ValidType.ALPHABETICAL_WITH_SPACES)]
        [Display(Name = "Nombre(s)")]
        public string Name { get; set; }

        [Display(Name = "Rol")]
        [Required]
        public int Role { get; set; }

        public IEnumerable<SelectListItem> Roles { get; set; }

        [Required]
        public string UserName { get; set; }

        //Attributes Custom
        [Display(Name="Cedis")]
        [Required]
        public int Cedis { get; set; }

        public IEnumerable<SelectListItem> CedisList { get; set; }

        [Display(Name="Estatus")]
        public bool Status { get; set; }

        public bool IsNew
        {
            get {
                return !(Uuid != null && Uuid.Trim().Length > 0);
            }
        }

        public string Uuid { get; set; }

    }

    public class UserRoleViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Nombre")]
        public string Name { get; set; }
    }
    
}