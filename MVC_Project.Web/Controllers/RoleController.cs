using MVC_Project.Data.Helpers;
using MVC_Project.Domain.Entities;
using MVC_Project.Domain.Services;
using MVC_Project.Web.Models;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Project.Web.Controllers
{
    public class RoleController : BaseController
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
            RoleViewModel roles = new RoleViewModel();
            //var roles = _roleService.GetAll().Select(role => new RoleViewModel
            //{
            //    Id = role.Id,
            //    Name = role.Name,
            //    Description = role.Description,
            //    CreatedAt = role.CreatedAt,
            //    UpdatedAt = role.UpdatedAt
            //});
            return View(roles);
        }
        [HttpGet, Authorize]
        public JsonResult ObtenerRoles(JQueryDataTableParams param, string filtros)
        {
            DataTableUsersModel model = new DataTableUsersModel();
            try
            {
                var roles = _roleService.ObtenerRoles(filtros);
                IList<RoleData> UsuariosResponse = new List<RoleData>();
                foreach (var rol in roles)
                {
                    RoleData userData = new RoleData();
                    userData.Name = rol.Name;
                    userData.Description = rol.Description;
                    userData.CreatedAt = rol.CreatedAt;
                    userData.UpdatedAt = rol.UpdatedAt;
                    userData.Status = rol.Status;
                    userData.Uuid = rol.Uuid;
                    UsuariosResponse.Add(userData);
                }
                return Json(new
                {
                    success = true,
                    sEcho = param.sEcho,
                    iTotalRecords = UsuariosResponse.Count(),
                    iTotalDisplayRecords = 10,
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
        public ActionResult Edit(string uuid)
        {
            Role role = new Role();
            role = _roleService.FindBy(x => x.Uuid == uuid).First();
            RoleEditViewModel model = new RoleEditViewModel();
            model.Name = role.Name;
            IEnumerable<PermissionViewModel> permisos = PopulatePermissions();
            foreach (PermissionViewModel permiso in permisos)
            {
                if (role.Permissions.Select(x => x.Description.Equals(permiso.Description)).First())
                {
                    permiso.Assigned = true;
                }
            }
            model.Permissions = permisos;
            return View(model);
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
        public ActionResult Delete(string uuid, FormCollection collection)
        {
            try
            {
                var rol = _roleService.FindBy(x => x.Uuid == uuid).First();
                if (rol != null)
                {
                    if (rol.Status == true)
                    {
                        rol.Status = false;
                    }
                    else
                    {
                        rol.Status = true;
                    }

                }
                _roleService.Update(rol);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
