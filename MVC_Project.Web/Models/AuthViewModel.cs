using System.ComponentModel.DataAnnotations;

namespace MVC_Project.Web.Models
{
    public class AuthViewModel
    {
        [Display(Name = "Usuario")]
        [Required, EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Contraseña")]
        [Required, MinLength(8)]
        public string Password { get; set; }
    }
}