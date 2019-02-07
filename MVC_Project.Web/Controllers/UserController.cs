using MVC_Project.Domain.Entities;
using MVC_Project.Domain.Services;
using MVC_Project.Web.AuthManagement;
using MVC_Project.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var users = _userService.GetAll().Select(user => new UserViewModel
            {
                Id = user.Id,
                Name = user.FirstName,
                Email = user.Email,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            });
            return View(users);
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
            if(ModelState.IsValid)
            {
                // TODO: Add insert logic here
                var user = new User
                {
                    FirstName = userCreateViewModel.Name,
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
                return View();
            }
        }

        // GET: User/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: User/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: User/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: User/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}