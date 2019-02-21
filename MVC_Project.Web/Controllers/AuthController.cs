using MVC_Project.Data.Helpers;
using MVC_Project.Domain.Services;
using MVC_Project.Web.AuthManagement;
using MVC_Project.Web.AuthManagement.Models;
using MVC_Project.Web.Models;
using NHibernate;
using System.Linq;
using System.Web.Mvc;

namespace MVC_Project.Web.Controllers
{
    public class AuthController : BaseController
    {
        private IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(AuthViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _authService.Authenticate(model.Email, EncryptHelper.EncryptPassword(model.Password));
                if (user != null)
                {
                    AuthUser authUser = new AuthUser
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        Role = new Role
                        {
                            Code = user.Role.Code
                        },
                        Permissions = user.Permissions.Select(p => new Permission
                        {
                            Action = p.Action,
                            Controller = p.Controller
                        }).ToList()
                    };
                    UnitOfWork unitOfWork = new UnitOfWork();
                    ISession session = unitOfWork.Session;
                    Authenticator.StoreAuthenticatedUser(authUser);
                    if (!string.IsNullOrEmpty(Request.Form["ReturnUrl"]))
                    {
                        return Redirect(Request.Form["ReturnUrl"]);
                    }
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.Error = "El usuario no existe o contraseña inválida.";
                }
            }

            return View(model);
        }

        [AllowAnonymous]
        public ActionResult Logout()
        {
            Authenticator.RemoveAuthenticatedUser();
            return RedirectToAction("Login", "Auth");
        }
    }
}