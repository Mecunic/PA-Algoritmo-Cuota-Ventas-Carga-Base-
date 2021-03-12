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
                    userData.Name = user.Nombre;
                    userData.UserName = user.Usuario;
                    userData.Status = user.Status;
                    userData.Uuid = user.Uuid;
                    //userData.CedisName = user.Cedis?.Nombre;
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
            if (uuid != null)
            {
                var user = _userService.FindBy(u => u.Uuid.Equals(uuid)).FirstOrDefault();
                if (user != null)
                {
                    userCreateViewModel.Uuid = user.Uuid;
                    userCreateViewModel.Name = user.Nombre;
                    userCreateViewModel.Role = user.Role.Id;
                    userCreateViewModel.Status = user.Status;
                    userCreateViewModel.UserName = user.Usuario;
                    //userCreateViewModel.Cedis = user.Cedis != null ? user.Cedis.Id : 0;
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
            var availableCedis = _cedisService.GetAll();
            var cedisList = new List<SelectListItem>();
            cedisList = availableCedis.Select(cedis => new SelectListItem
            {
                Value = cedis.Id.ToString(),
                Text = cedis.Nombre
            }).ToList();
            return cedisList;
        }

    }
}