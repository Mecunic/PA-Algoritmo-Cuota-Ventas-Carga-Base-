using MVC_Project.Domain.Services;
using MVC_Project.Web.AuthManagement;
using MVC_Project.Web.AuthManagement.Models;
using MVC_Project.Web.Models;
using System.Linq;
using System.Web.Mvc;

namespace MVC_Project.Web.Controllers
{

    public class AuthController : Controller
    {
        private IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(AuthViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _authService.Authenticate(model.Email, model.Password);
                if (user != null)
                {
                    AuthUser authUser = new AuthUser
                    {
                        Email = user.Email,
                        Role = new Role
                        {
                            Code = user.Role.Code
                        },
                        Permissions = user.Permissions.Select(p => new Permission {
                            Action = p.Action,
                            Controller = p.Controller
                        }).ToList()
                    };
                    Authenticator.StoreAuthenticatedUser(authUser);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.Error = "El usuario no existe o contraseña inválida.";
                }
            }

            return View(model);
        }
    }
}