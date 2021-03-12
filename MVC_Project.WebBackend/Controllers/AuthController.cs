using MVC_Project.Domain.Services;
using MVC_Project.Resources;
using MVC_Project.Utils;
using MVC_Project.WebBackend.App_Code;
using MVC_Project.WebBackend.AuthManagement;
using MVC_Project.WebBackend.AuthManagement.Models;
using MVC_Project.BackendWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MVC_Project.WebBackend.Models;
using MVC_Project.WebApis.Servicios;

namespace MVC_Project.WebBackend.Controllers
{
    public class AuthController : BaseController
    {
        private IAuthService _authService;
        private IUserService _userService;
        private IPermissionService _permissionService;

        public AuthController(IAuthService authService, IUserService userService, IPermissionService permissionService)
        {
            _authService = authService;
            _userService = userService;
            _permissionService = permissionService;
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            var model = new AuthViewModel
            {
                AvailableCedis = SetAvailableCedis()
            };
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(AuthViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _authService.Authenticate(model.Username, model.Password, model.IdCedis);
                if (user != null)
                {
                    if (!user.Status)
                    {
                        ViewBag.Error = Resources.ErrorMessages.UserInactive;
                        model.AvailableCedis = SetAvailableCedis(model.IdCedis);
                        return View(model);
                    }
                    _userService.Update(user);

                    List<Permission> permissionsUser = user.Role.Permissions.Select(p => new Permission
                    {
                        Action = p.Action,
                        Controller = p.Controller,
                        Module = p.Module
                    }).ToList();

                    AuthUser authUser = new AuthUser
                    {
                        Id = user.Id,
                        FirstName = user.Nombre,
                        Uuid = user.Uuid,
                        Role = new Role
                        {
                            Code = user.Role.Code,
                            Name = user.Role.Name
                        },
                        Permissions = permissionsUser
                    };

                    //Set user in sesion
                    Authenticator.StoreAuthenticatedUser(authUser);

                    //Set Language
                    LanguageMngr.SetDefaultLanguage();
                    if (!string.IsNullOrEmpty(authUser.Language))
                    {
                        LanguageMngr.SetLanguage(authUser.Language);
                    }

                    if (!string.IsNullOrEmpty(Request.Form["ReturnUrl"]))
                    {
                        return Redirect(Request.Form["ReturnUrl"]);
                    }
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    AddViewMessage(TypeMessageView.ERROR, ErrorMessages.UserNotExistsOrPasswordInvalid);
                }
            }
            model.AvailableCedis = SetAvailableCedis(model.IdCedis);
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult Logout()
        {
            Authenticator.RemoveAuthenticatedUser();
            return RedirectToAction("Login", "Auth");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ChangeLanguage(string lang)
        {
            LanguageMngr.SetLanguage(lang);
            return RedirectToAction("Index", "Home");
        }

        private IEnumerable<SelectListItem> SetAvailableCedis(int? selected = null)
        {
            var cedisResponse = IntermediaService.Cedis();
            var items = new List<SelectListItem>();
            foreach (var cedis in cedisResponse)
            {
                items.Add(new SelectListItem
                {
                    Value = cedis.CedisIdOpecd.ToString(),
                    Text = cedis.Nombre,
                    Selected = cedis.CedisIdOpecd == selected
                });
            }
            return items;
        }
    }
}