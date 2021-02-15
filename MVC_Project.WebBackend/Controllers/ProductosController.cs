using MVC_Project.Domain.Services;
using MVC_Project.WebBackend.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Project.WebBackend.Controllers
{
    public class ProductosController : BaseController
    {
        private IProductoService _ProductoService;
        public ProductosController(IProductoService productoService)
        {
            this._ProductoService = productoService;
        }
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
                //var results = _userService.FilterBy(filtersValues, param.iDisplayStart, param.iDisplayLength);
                IList<ProductoModel> dataResponse = new List<ProductoModel>();
                /*foreach (var user in results.Item1)
                {
                    UserData userData = new UserData();
                    userData.Name = user.FirstName + " " + user.ApellidoPaterno;
                    userData.Email = user.Email;
                    userData.Status = user.Status;
                    userData.Uuid = user.Uuid;
                    userData.CedisName = user.Cedis?.Name;
                    dataResponse.Add(userData);
                }*/
                return Json(new
                {
                    success = true,
                    param.sEcho,
                    iTotalRecords = dataResponse.Count(),
                    iTotalDisplayRecords = 0,
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
    }
}
