using MVC_Project.Data.Helpers;
using MVC_Project.Domain.Entities;
using MVC_Project.Domain.Services;
using MVC_Project.Utils;
using MVC_Project.Web.AuthManagement;
using MVC_Project.Web.AuthManagement.Models;
using MVC_Project.Web.Models;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;

namespace MVC_Project.Web.Controllers
{
    public class AuthController : BaseController
    {
        private IAuthService _authService;
        private UserService _userService;
        private RoleService _roleService;

        public AuthController(IAuthService authService, UserService userService, RoleService roleService)
        {
            _authService = authService;
            _userService = userService;
            _roleService = roleService;
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
                User user = _authService.Authenticate(model.Email, EncryptHelper.EncryptPassword(model.Password));
                Domain.Entities.Role role = _roleService.FindBy(x => x.Id == user.Role.Id).First();
                if (user != null)
                {
                    AuthUser authUser = new AuthUser
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        Role = new AuthManagement.Models.Role
                        {
                            Code = user.Role.Code
                        },
                        Permissions = role.Permissions.Select(p => new AuthManagement.Models.Permission
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
        [AllowAnonymous]
        public ActionResult RecuperarContrasena()
        {
            ViewBag.mensajeError = string.Empty;
            return PartialView("RecuperarContrasena");
        }
        [HttpPost, AllowAnonymous]
        public ActionResult RecuperarContrasena(RecoverPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new
                {
                    success = false,
                    issue = model,
                    errors = ModelState.Keys.Where(k => ModelState[k].Errors.Count > 0)
                    .Select(k => new { propertyName = k, errorMessage = ModelState[k].Errors[0].ErrorMessage })
                });
            }
            try
            {
                var resultado = _userService.FindBy(e => e.Email == model.Email).First();
                if (resultado != null)
                {
                    ViewBag.mensajeError = string.Empty;
                    resultado.ExpiraToken = System.DateTime.Now.AddDays(1);
                    string token = (resultado.Uuid + "@" + DateTime.Now.AddDays(1).ToString());
                     token = EncryptorText.DataEncrypt(token).Replace("/", "!!").Replace("+", "$");
                    resultado.Token = token;
                    List<string> Email = new List<string>();
                    Email.Add(resultado.Email);
                    Dictionary<string, string> customParams = new Dictionary<string, string>();
                    string urlAccion = (string)ConfigurationManager.AppSettings["_UrlServerAccess"];
                    string link = urlAccion + "Auth/AccedeToken?token=" + token;
                    customParams.Add("param1", resultado.Email);
                    customParams.Add("param2", link);
                    string template = "aa61890e-5e39-43c4-92ff-fae95e03a711";
                    NotificationUtil.SendNotification(Email, customParams, template);
                    _userService.Update(resultado);

                    //MensajesFlash.MensajeFlashHandler.RegistrarMensaje(ImpuestoPredial.Resource.Recursos.OperacionExitosa);
                    ViewBag.Message = "Solicitud realizada";
                    return View("Login");

                }
            }
            catch (Exception ex)
            {
                //ErrorController.SaveLogError(this, listAction.Update, "RecuperarContrasena", ex);
            }

            ModelState.AddModelError("Email", "No se encontró ninguna cuenta con el correo proporcionado. Verifique su información.");
            return Json(new
            {
                success = false,
                issue = model,
                errors = ModelState.Keys.Where(k => ModelState[k].Errors.Count > 0)
                .Select(k => new { propertyName = k, errorMessage = ModelState[k].Errors[0].ErrorMessage })
            });

        }
        [AllowAnonymous]
        public ActionResult AccedeToken(string token)
        {
            try
            {
                if (string.IsNullOrEmpty(token))
                    return RedirectToAction("Login");

                var desencriptaToken = EncryptorText.DataDecrypt(token.Replace("!!", "/").Replace("$", "+"));

                if (string.IsNullOrEmpty(desencriptaToken))
                    return RedirectToAction("Login");

                var elements = desencriptaToken.Split('@');
                string id = elements.First().ToString();
                var resultado = _userService.FindBy(e => e.Uuid == id).First();
                int[] valores = new int[100];
                for(int a=0;a<100; a++)
                {
                    valores[a] = a++;
                }
                if (resultado != null && DateTime.Now <= resultado.ExpiraToken)
                {
                    ResetPassword model = new ResetPassword();
                    model.Uuid = resultado.Uuid.ToString();
                    return View("ResetPassword", model);
                }
                else
                {
                    ViewBag.Message = "Token de contraseña expirado";
                    return View("Login");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Token de contraseña expirado";
                return View("Login");
                //ErrorController.SaveLogError(this, listAction.Update, "AccedeToken", ex);
            }
            ViewBag.Message = "Error en el token";
            return View("Login");
        }
        [HttpPost, AllowAnonymous]
        public ActionResult Reset(ResetPassword model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new
                {
                    success = false,
                    issue = model,
                    errors = ModelState.Keys.Where(k => ModelState[k].Errors.Count > 0)
                    .Select(k => new { propertyName = k, errorMessage = ModelState[k].Errors[0].ErrorMessage })
                });
            }
            try
            {
                var resultado = _userService.FindBy(e => e.Uuid == model.Uuid).First();
                if (resultado != null)
                {
                    resultado.Password = model.Password;

                    _userService.Update(resultado);
                    AuthUser authUser = new AuthUser
                    {
                        FirstName = resultado.FirstName,
                        LastName = resultado.LastName,
                        Email = resultado.Email,
                        Role = new AuthManagement.Models.Role
                        {
                            Code = resultado.Role.Code
                        },
                        Permissions = resultado.Permissions.Select(p => new AuthManagement.Models.Permission
                        {
                            Action = p.Action,
                            Controller = p.Controller
                        }).ToList()
                    };
                    UnitOfWork unitOfWork = new UnitOfWork();
                    ISession session = unitOfWork.Session;
                    Authenticator.StoreAuthenticatedUser(authUser);
                    return RedirectToAction("Index", "Home");

                }
            }
            catch (Exception ex)
            {
                //ErrorController.SaveLogError(this, listAction.Update, "RecuperarContrasena", ex);
            }

            ModelState.AddModelError("Password", "No se encontró ninguna cuenta con el correo proporcionado. Verifique su información.");
            return View("ResetPassword", model);
        }
    }
}