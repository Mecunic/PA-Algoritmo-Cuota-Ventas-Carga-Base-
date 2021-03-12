using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using MVC_Project.Resources;

namespace MVC_Project.BackendWeb.Models
{
    public class AuthViewModel
    {
        [Display(Name = "USERNAME", ResourceType = typeof(ViewLabels))]
        [Required(ErrorMessageResourceType = typeof(ViewLabels), ErrorMessageResourceName = "UsernameRequired")]
        public string Username { get; set; }

        [Display(Name = "PASSWORD", ResourceType = typeof(ViewLabels))]
        [Required(ErrorMessageResourceType = typeof(ViewLabels), ErrorMessageResourceName = "PasswordRequired")]
        public string Password { get; set; }

        [Display(Name = "CEDIS", ResourceType = typeof(ViewLabels))]
        [Required(ErrorMessageResourceType = typeof(ViewLabels), ErrorMessageResourceName = "CedisRequired")]
        public int IdCedis { get; set; }

        public IEnumerable<SelectListItem> AvailableCedis { get; set; }
    }
}