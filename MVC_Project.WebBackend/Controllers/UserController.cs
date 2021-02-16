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
using MVC_Project.WebBackend.Utils;

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

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
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

        public ActionResult Create(string uuid = null)
        {
            var userCreateViewModel = new UserSaveViewModel 
            { Roles = PopulateRoles(), CedisList = PopulateCedis() };
            if(uuid != null)
            {
                var user = _userService.FindBy(u => u.Uuid.Equals(uuid)).FirstOrDefault();
                if(user != null)
                {
                    userCreateViewModel.Uuid = user.Uuid;
                    userCreateViewModel.Name = user.FirstName;
                    userCreateViewModel.Role = user.Role.Id;
                    userCreateViewModel.Status = user.Status;
                    userCreateViewModel.Email = user.Email;
                    userCreateViewModel.ApellidoPaterno = user.ApellidoPaterno;
                    userCreateViewModel.ApellidoMaterno = user.ApellidoMaterno;
                    userCreateViewModel.Cedis = user.Cedis != null ? user.Cedis.Id : 0;
                    userCreateViewModel.Status = user.Status;
                }
            }
            return PartialView(userCreateViewModel);
        }

        private IEnumerable<SelectListItem> PopulateRoles()
        {
            var availableRoles = _roleService.GetAll().Where(c => c.Status == true); ;
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
            var availableCedis = _cedisService.GetAll().Where(c=>c.Status == true);
            var cedisList = new List<SelectListItem>();
            cedisList = availableCedis.Select(cedis => new SelectListItem
            {
                Value = cedis.Id.ToString(),
                Text = cedis.Name
            }).ToList();
            return cedisList;
        }

        [HttpPost, ValidateAntiForgeryToken,ValidateInput(true)]
        public ActionResult Create(UserSaveViewModel model)
        {
            ValidationModel(model);
            if (ModelState.IsValid)
            {
                DateTime todayDate = DateUtil.GetDateTimeNow();

                if (model.IsNew)
                {
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
                        Password = SecurityUtil.EncryptPassword(model.Password),
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
                    string successMessage = "Usuario Creado";
                    return Json(new
                    {
                        Message = successMessage
                    });
                }
                else
                {
                    var user = _userService.FindBy(u => u.Uuid.Equals(model.Uuid)).FirstOrDefault();
                    user.FirstName = model.Name;
                    user.ApellidoMaterno = model.ApellidoMaterno;
                    user.ApellidoPaterno = model.ApellidoPaterno;
                    user.Role = new Role { Id = model.Role };
                    var role = _roleService.GetById(user.Role.Id);
                    user.Status = model.Status;
                    foreach (var permission in role.Permissions)
                    {
                        user.Permissions.Add(permission);
                    }
                    _userService.Update(user);
                    string successMessage = "Usuario Actualizado";
                    return Json(new
                    {
                        Message = successMessage
                    });
                }
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

        
        [HttpPost]
        public ActionResult Delete(string uuid)
        {
            try
            {
                var user = _userService.FindBy(x => x.Uuid == uuid).First();
                if (user != null)
                {
                    user.Status = false;
                    user.RemovedAt = DateTime.Now;
                    _userService.Update(user);
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        private void ValidationModel(UserSaveViewModel model)
        {
            bool validEmail = true;
            if (!model.IsNew)
            {
                validEmail = false;
            }
            else
            {
                var password = model.Password;
                if(password != null && (password.Trim().Length <8 || password.Trim().Length > 20))
                {
                    ModelState.AddModelError("Password", "El campo Contraseña debe ser una cadena con una longitud mínima de 8 y una longitud máxima de 20.");
                }
                else
                {
                    ModelState.AddModelError("Password", "El campo Contraseña es obligatorio");
                }
                if (!password.ContainsCapitalLetter() || !password.ContainsNumbers() || !password.ContainsCaractersSpecial())
                {
                    ModelState.AddModelError("Password", "El Contraseña debe contener una letra Mayuscula, un número y un caracter especial.");
                }
            }
            if (validEmail && (model.Email == null || model.Email.Trim().Length == 0) )
            {
                ModelState.AddModelError("Email", "El Email es requerido.");
            }
            if (model.Email != null)
            {
                if (validEmail && _userService.Exists(model.Email))
                {
                    ModelState.AddModelError("Email", "El Usuario ya se encuentra registrado.");
                }
            }

        }


    }
}