using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Project.Web.Models {

    public class UserViewModel {
        public int Id { get; set; }

        [Display(Name = "Nombre")]
        public string Name { get; set; }

        public string Uuid { get; set; }

        public string Email { get; set; }

        [Display(Name = "Fecha de creación")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Fecha de actualización")]
        public DateTime UpdatedAt { get; set; }
    }

    public class UserCreateViewModel {

        [Display(Name = "Nombre")]
        public string Name { get; set; }

        public string Email { get; set; }

        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Display(Name = "Rol")]
        public int Role { get; set; }

        public IEnumerable<SelectListItem> Roles { get; set; }
    }

    public class UserRoleViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Nombre")]
        public string Name { get; set; }
    }
}