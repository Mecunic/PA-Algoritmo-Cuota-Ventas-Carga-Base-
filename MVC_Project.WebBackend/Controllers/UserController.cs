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
        private ICedisService _cedisService;

        public UserController(IUserService userService, IRoleService roleService, ICedisService cedisService = null)
        {
            _userService = userService;
            _roleService = roleService;
            _cedisService = cedisService;
        }

        [Authorize]
        public ActionResult Index()
        {
            return View();
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
                    userData.Name = user.FirstName + " " + user.ApellidoPaterno;
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
            var userCreateViewModel = new UserCreateViewModel 
            { Roles = PopulateRoles(), CedisList = PopulateCedis() };
            return PartialView(userCreateViewModel);
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

        private IEnumerable<SelectListItem> PopulateCedis()
        {
            var availableCedis = _cedisService.GetAll();
            var cedisList = new List<SelectListItem>();
            cedisList = availableCedis.Select(cedis => new SelectListItem
            {
                Value = cedis.Id.ToString(),
                Text = cedis.Name
            }).ToList();
            return cedisList;
        }

        [HttpPost, ValidateAntiForgeryToken,ValidateInput(true)]
        public ActionResult Create(UserCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                DateTime todayDate = DateUtil.GetDateTimeNow();

                string daysToExpirateDate = ConfigurationManager.AppSettings["DaysToExpirateDate"];

                DateTime passwordExpiration = todayDate.AddDays(Int32.Parse(daysToExpirateDate));
                var user = new User
                {
                    Uuid = Guid.NewGuid().ToString(),
                    FirstName = model.Name,
                    ApellidoPaterno = model.ApellidoPaterno,
                    ApellidoMaterno = model.ApellidoMaterno,
                    //LastName = userCreateViewModel.Apellidos,
                    Email = model.Email,
                    //MobileNumber = userCreateViewModel.MobileNumber,
                    Password = SecurityUtil.EncryptPassword("12345678"),
                    PasswordExpiration = passwordExpiration,
                    Role = new Role { Id = model.Role },
                    //Username = model.Username,
                    Cedis = new Cedis { Id = model.Cedis },
                    //Language = userCreateViewModel.Language,
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
                string successMessage = Resources.Messages.UserPasswordUpdated;
                return Json(new
                {
                    Message = successMessage
                });
            }
            else
            {
                Response.StatusCode = 422;
                return Json(new
                {
                    issue = model,
                    errors = ModelState.Keys.Where(k => ModelState[k].Errors.Count > 0)
                    .Select(k => new { propertyName = k, errorMessage = ModelState[k].Errors[0].ErrorMessage })
                });
            }
        }

        [Authorize]
        public ActionResult Edit(string uuid)
        {
            User user = _userService.FindBy(x => x.Uuid == uuid).First();
            UserEditViewModel model = new UserEditViewModel();
            model.Uuid = user.Uuid;
            model.Name = user.FirstName;
            model.Apellidos = user.ApellidoPaterno;
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
                user.ApellidoPaterno = model.Apellidos;
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
        public ActionResult Delete(string uuid)
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