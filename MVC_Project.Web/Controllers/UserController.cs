using MVC_Project.Data.Helpers;
using MVC_Project.Domain.Entities;
using MVC_Project.Domain.Helpers;
using MVC_Project.Domain.Services;
using MVC_Project.Web.AuthManagement;
using MVC_Project.Web.Models;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;

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
                UserList = new UserData()
            };
            List<SelectListItem> listStatus = new List<SelectListItem>();
            listStatus.Add(new SelectListItem
            {
                Text = "Todos",
                Value = "2"
            });
            listStatus.Add(new SelectListItem
            {
                Text = "Activos",
                Value = "1",
                Selected = true
            });
            listStatus.Add(new SelectListItem
            {
                Text = "Inactivos",
                Value = "0"
            });
            ViewBag.OpcionesStatus = listStatus;
            return View(model);
        }
        [HttpGet, Authorize]
        public JsonResult ObtenerUsuarios(JQueryDataTableParams param, string filtros)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork();
                ISession session = unitOfWork.Session;
                var users = _userService.ObtenerUsuarios(filtros, session);
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
                        UsuariosResponse.Add(userData);
                    }
                    return Json(new
                    {
                        success = true,
                        sEcho = param.sEcho,
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
            try
            {
                var userCreateViewModel = new UserCreateViewModel { Roles = PopulateRoles() };
                return View("Form", userCreateViewModel);
            }
            catch
            {
                return View();
            }
           
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
            if(ModelState.IsValid)
            {
                // TODO: Add insert logic here
                var user = new User
                {
                    Uuid = Guid.NewGuid().ToString(),
                    FirstName = userCreateViewModel.Name,
                    LastName = userCreateViewModel.Apellidos,
                    Email = userCreateViewModel.Email,
                    Password = EncryptHelper.EncryptPassword(userCreateViewModel.Password),
                    Role = new Role { Id = userCreateViewModel.Role },
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,                             
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
                return View("Form", userCreateViewModel);
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
            return View("EditForm",model);
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
                _userService.Update(user);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(model);
            }
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
                var users = _userService.FindBy(x=>x.Uuid == uuid).First();
                if (users != null)
                {
                    if(users.Status == true)
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
            catch(Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult SearchUsers(UserViewModel users)
        {
            //try
            //{
            //    var user = _userService.ObtenerUsuarios(users.Name, 1, 0,0);
            //}
            //catch (Exception ex)
            //{
            //    return Json(false, JsonRequestBehavior.AllowGet);
            //}
            return null;
        }
    }
}