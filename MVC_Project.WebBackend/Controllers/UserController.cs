using ExcelEngine;
using MVC_Project.Domain.Entities;
using MVC_Project.Domain.Services;
using MVC_Project.Utils;
using MVC_Project.WebBackend.Models;
using MVC_Project.Web.Models.ExcelImport;
using MVC_Project.WebBackend.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Project.WebBackend.Controllers
{
    public class UserController : BaseController
    {
        private IUserService _userService;
        private IRoleService _roleService;

        public UserController(IUserService userService, IRoleService roleService)
        {
            _userService = userService;
            _roleService = roleService;
        }

        [Authorize]
        public ActionResult Index()
        {
            UserViewModel model = new UserViewModel
            {
                UserList = new UserData(),
                Status = FilterStatusEnum.ALL.Id,
                Statuses = FilterStatusEnum.GetSelectListItems()
            };
            return View(model);
        }

        [HttpGet, Authorize]
        public JsonResult GetAllByFilter(JQueryDataTableParams param, string filtros)
        {
            try
            {
                NameValueCollection filtersValues = HttpUtility.ParseQueryString(filtros);
                var results = _userService.FilterBy(filtersValues, param.iDisplayStart, param.iDisplayLength);
                IList<UserData> dataResponse = new List<UserData>();
                foreach (var user in results.Item1)
                {
                    UserData userData = new UserData();
                    userData.Name = user.FirstName + " " + user.LastName;
                    userData.Email = user.Email;
                    userData.Status = user.Status;
                    userData.Uuid = user.Uuid;
                    userData.CedisName = user.Cedis?.Name;
                    dataResponse.Add(userData);
                }
                return Json(new
                {
                    success = true,
                    param.sEcho,
                    iTotalRecords = dataResponse.Count(),
                    iTotalDisplayRecords = results.Item2,
                    aaData = dataResponse
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return new JsonResult
                {
                    Data = new { Mensaje = new { title = "Error", message = ex.Message } },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    MaxJsonLength = Int32.MaxValue
                };
            }
        }

        [Authorize]
        public ActionResult Create()
        {
            var userCreateViewModel = new UserCreateViewModel { Roles = PopulateRoles() };
            return View(userCreateViewModel);
        }

        private IEnumerable<SelectListItem> PopulateRoles()
        {
            var availableRoles = _roleService.GetAll();
            var rolesList = new List<SelectListItem>();
            rolesList = availableRoles.Select(role => new SelectListItem
            {
                Value = role.Id.ToString(),
                Text = role.Name
            }).ToList();
            return rolesList;
        }

        [Authorize, HttpPost, ValidateAntiForgeryToken, ValidateInput(true)]
        public ActionResult Create(UserCreateViewModel userCreateViewModel)
        {
            if(!String.IsNullOrWhiteSpace(userCreateViewModel.ConfirmPassword) 
                && !String.IsNullOrWhiteSpace(userCreateViewModel.Password))
            {
                if(!userCreateViewModel.Password.Equals(userCreateViewModel.ConfirmPassword))
                {
                    ModelState.AddModelError("ConfirmPassword", "Las contraseñas no coinciden");
                }
            }
            if (ModelState.IsValid)
            {
                DateTime todayDate =  DateUtil.GetDateTimeNow();

                string daysToExpirateDate = ConfigurationManager.AppSettings["DaysToExpirateDate"];
                
                DateTime passwordExpiration = todayDate.AddDays(Int32.Parse(daysToExpirateDate));
                var user = new User
                {
                    Uuid = Guid.NewGuid().ToString(),
                    FirstName = userCreateViewModel.Name,
                    LastName = userCreateViewModel.Apellidos,
                    Email = userCreateViewModel.Email,
                    MobileNumber = userCreateViewModel.MobileNumber,
                    Password = SecurityUtil.EncryptPassword(userCreateViewModel.Password),
                    PasswordExpiration = passwordExpiration,
                    Role = new Role { Id = userCreateViewModel.Role },
                    Username = userCreateViewModel.Username,
                    Language = userCreateViewModel.Language,
                    CreatedAt = todayDate,
                    UpdatedAt = todayDate,
                    Status = true
                };
                var role = _roleService.GetById(user.Role.Id);
                foreach (var permission in role.Permissions)
                {
                    user.Permissions.Add(permission);
                }
                _userService.Create(user);
                return RedirectToAction("Index");
            }
            else
            {
                userCreateViewModel.Roles = PopulateRoles();
                return View("Create", userCreateViewModel);
            }
        }

        [Authorize]
        public ActionResult Edit(string uuid)
        {
            User user = _userService.FindBy(x => x.Uuid == uuid).First();
            UserEditViewModel model = new UserEditViewModel();
            model.Uuid = user.Uuid;
            model.Name = user.FirstName;
            model.Apellidos = user.LastName;
            model.Email = user.Email;
            model.MobileNumber = user.MobileNumber;
            model.Roles = PopulateRoles();
            model.Role = user.Role.Id;
            return View(model);
        }

        [Authorize, HttpPost, ValidateAntiForgeryToken, ValidateInput(true)]
        public ActionResult Edit(UserEditViewModel model, FormCollection collection)
        {
            try
            {
                User user = _userService.FindBy(x => x.Uuid == model.Uuid).First();
                user.FirstName = model.Name;
                user.LastName = model.Apellidos;
                user.Email = model.Email;
                user.MobileNumber = model.MobileNumber;
                user.Username = model.Username;
                user.Language = model.Language;
                _userService.Update(user);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(model);
            }
        }
        
        [HttpPost, Authorize]
        public ActionResult Delete(string uuid, FormCollection collection)
        {
            try
            {
                var user = _userService.FindBy(x => x.Uuid == uuid).First();
                if (user != null)
                {
                    user.Status = false;
                    _userService.Update(user);
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
    }
}