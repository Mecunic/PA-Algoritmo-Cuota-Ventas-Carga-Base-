using MVC_Project.Domain.Services;
using MVC_Project.Web.Models;
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

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(AuthViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (_authService.Authenticate(model.Email, model.Password))
                {
                    return RedirectToAction("Index", "Home");
                } else
                {
                    ViewBag.Error = "El usuario no existe o contraseña inválida.";
                }                
            }

            return View(model);
        }
    }
}