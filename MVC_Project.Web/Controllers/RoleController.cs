using MVC_Project.Domain.Services;
using MVC_Project.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Project.Web.Controllers
{
    public class RoleController : Controller
    {
        private RoleService _roleService;
        private PermissionService _permissionService;
        public RoleController(RoleService roleService, PermissionService permissionService)
        {
            _roleService = roleService;
            _permissionService = permissionService;
        }

        // GET: Role
        public ActionResult Index()
        {
            var roles = _roleService.GetAll().Select(role => new RoleViewModel
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description,
                CreatedAt = role.CreatedAt,
                UpdatedAt = role.UpdatedAt
            });
            return View(roles);
        }

        // GET: Role/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Role/Create
        public ActionResult Create()
        {
            var roleCreateViewModel = new RoleCreateViewModel { Permissions = PopulatePermissions()};
            return View(roleCreateViewModel);
        }

        private IEnumerable<PermissionViewModel> PopulatePermissions()
        {
            var permissions = _permissionService.GetAll();
            var permissionsVM = new List<PermissionViewModel>();
            permissionsVM = permissions.Select(permission => new PermissionViewModel
            {
                Id = permission.Id,
                Description = permission.Description
            }).ToList();
            return permissionsVM;
        }

        // POST: Role/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        // GET: Role/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Role/Edit/5
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

        // GET: Role/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Role/Delete/5
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
