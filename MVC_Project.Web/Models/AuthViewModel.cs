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
    public class RecoverPasswordViewModel
    {
        [Required]
        [Display(Name = "Correo electrónico ")]
        [EmailAddress]
        [RegularExpression("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9-]+(\\.[a-z0-9]+)*\\.([a-z]{2,6})$", ErrorMessage = "El Email no es válido")]
        public string Email { get; set; }
    }
    public class ResetPassword
    {
        public string Uuid { get; set; }
        [Display(Name = "Contraseña")]
        [Required, MinLength(8)]
        public string Password { get; set; }
        [Display(Name = "Confirmar contraseña")]
        [Required, MinLength(8)]
        public string NewPassword { get; set; }
    }
}