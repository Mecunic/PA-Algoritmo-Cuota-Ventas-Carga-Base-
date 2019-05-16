using ExcelEngine;
using MVC_Project.Data.Helpers;
using MVC_Project.Domain.Entities;
using MVC_Project.Domain.Helpers;
using MVC_Project.Domain.Services;
using MVC_Project.Web.AuthManagement;
using MVC_Project.Web.Models;
using MVC_Project.Web.Models.ExcelImport;
using MVC_Project.Web.Utils.Enums;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;
using Utils;

namespace MVC_Project.Web.Controllers
{
    public class UserController : BaseController
    {
        private UserService _userService;
        private RoleService _roleService;

        public UserController(UserService userService, RoleService roleService)
        {
            _userService = userService;
            _roleService = roleService;
        }

        // GET: User
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
        [Authorize]
        public ActionResult Import()
        {
            UserImportViewModel model = new UserImportViewModel();
            return View("Import", model);
        }
        [HttpPost, Authorize, ValidateAntiForgeryToken]
        public ActionResult Import(UserImportViewModel model)
        {
            if (ModelState.IsValid)
            {
                ResultExcelImporter<UserImport> result = ExcelImporterMapper.ReadExcel<UserImport>(new ExcelFileInputData
                {
                    ContentLength = model.ImportedFile.ContentLength,
                    FileName = model.ImportedFile.FileName,
                    InputStream = model.ImportedFile.InputStream
                });
                model.ImportResult = new List<UserRowImportResultViewModel>();
                foreach (RowResult rowResult in result.ResultMapExcel.RowResults)
                {
                    UserRowImportResultViewModel userRowImportResultViewModel = new UserRowImportResultViewModel();
                    userRowImportResultViewModel.Email = rowResult.RowsValues.Email;
                    userRowImportResultViewModel.EmployeeNumber = rowResult.RowsValues.EmployeeNumber;
                    userRowImportResultViewModel.Name = rowResult.RowsValues.Name;
                    userRowImportResultViewModel.RowNumber = rowResult.Number;
                    userRowImportResultViewModel.Messages = new List<string>();
                    bool hasCustomError = false;
                    User existingUser = _userService.FindBy(x => x.Email == userRowImportResultViewModel.Email).FirstOrDefault();
                    if (existingUser != null && !String.IsNullOrWhiteSpace(userRowImportResultViewModel.Email))
                    {
                        userRowImportResultViewModel.Messages.Add("El correo electrónico del usuario ya se encuentra registrado");
                    }
                    hasCustomError = userRowImportResultViewModel.Messages.Any();
                    if (rowResult.HasError)
                    {
                        userRowImportResultViewModel.Messages = userRowImportResultViewModel.Messages.Concat(rowResult.ErrorMessages).ToList();
                    }
                    if(!rowResult.HasError && !hasCustomError)
                    {
                        
                        userRowImportResultViewModel.Messages.Add("Usuario registrado satisfactoriamente");
                    }
                }
            }
            return View("Import", model);
        }

        [HttpGet, Authorize]
        public JsonResult GetAllByFilter(JQueryDataTableParams param, string filtros)
        {
            try
            {
                var users = _userService.FilterBy(filtros);
                IList<UserData> UsuariosResponse = new List<UserData>();

                foreach (var user in users)
                {
                    UserData userData = new UserData();
                    userData.Name = user.FirstName + " " + user.LastName;
                    userData.Email = user.Email;
                    userData.RoleName = user.Role.Name;
                    userData.CreatedAt = user.CreatedAt;
                    userData.UpdatedAt = user.UpdatedAt;
                    userData.Status = user.Status;
                    userData.Uuid = user.Uuid;
                    userData.LastLoginAt = user.LastLoginAt;
                    UsuariosResponse.Add(userData);
                }
                return Json(new
                {
                    success = true,
                    param.sEcho,
                    iTotalRecords = UsuariosResponse.Count(),
                    iTotalDisplayRecords = 20,
                    aaData = UsuariosResponse
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

        // GET: User/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: User/Create
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

        // POST: User/Create
        [HttpPost]
        public ActionResult Create(UserCreateViewModel userCreateViewModel)
        {
            if(!String.IsNullOrWhiteSpace(userCreateViewModel.ConfirmPassword) 
                && !String.IsNullOrWhiteSpace(userCreateViewModel.Password))
            {
                if(!userCreateViewModel.Password.Equals(userCreateViewModel.ConfirmPassword))
                {
                    ModelState.AddModelError("ConfirmPassword", "las cntraseñas no coinciden");
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
                    Password = EncryptHelper.EncryptPassword(userCreateViewModel.Password),
                    PasswordExpiration = passwordExpiration,
                    Role = new Role { Id = userCreateViewModel.Role },
                    Username = userCreateViewModel.Username,
                    Language = userCreateViewModel.Language,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
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

        // GET: User/Edit/5
        public ActionResult Edit(string uuid)
        {
            User user = _userService.FindBy(x => x.Uuid == uuid).First();
            UserEditViewModel model = new UserEditViewModel();
            model.Uuid = user.Uuid;
            model.Name = user.FirstName;
            model.Apellidos = user.LastName;
            model.Email = user.Email;
            model.Roles = PopulateRoles();
            model.Role = user.Role.Id;
            return View(model);
        }

        // POST: User/Edit/5
        [HttpPost]
        public ActionResult Edit(UserEditViewModel model, FormCollection collection)
        {
            try
            {
                User user = _userService.FindBy(x => x.Uuid == model.Uuid).First();
                user.FirstName = model.Name;
                user.LastName = model.Apellidos;
                user.Email = model.Email;
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
        [HttpGet]
        public ActionResult EditPassword()
        {
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult EditPassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {

            }
            return View(model);
        }

        // GET: User/Delete/5
        public ActionResult Delete(string uuid)
        {
            return View();
        }

        // POST: User/Delete/5
        [HttpPost]
        public ActionResult Delete(string uuid, FormCollection collection)
        {
            try
            {
                var users = _userService.FindBy(x => x.Uuid == uuid).First();
                if (users != null)
                {
                    if (users.Status == true)
                    {
                        users.Status = false;
                    }
                    else
                    {
                        users.Status = true;
                    }
                }
                _userService.Update(users);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
    }
}