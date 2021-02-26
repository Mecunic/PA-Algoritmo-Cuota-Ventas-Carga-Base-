using MVC_Project.WebBackend.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Project.WebBackend.Controllers
{
    public class ParametrosController : BaseController
    {
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
                //var results = _productoService.FilterBy(filtersValues, param.iDisplayStart, param.iDisplayLength);
                IList<ParametroModel> dataResponse = new List<ParametroModel>();
                /*foreach (var prod in results.Item1)
                {
                    ProductoModel productoModel = new ProductoModel();
                    productoModel.SKU = prod.SKU;
                    productoModel.TipoEmpaque = prod.TipoEmpaque?.Name;
                    productoModel.Presentacion = prod.Presentacion.Name;
                    productoModel.PrecioUnitario = prod.PrecioUnitario;
                    productoModel.Uuid = prod.Uuid;
                    productoModel.Status = prod.Status;
                    productoModel.Descripcion = prod.Descripcion;
                    dataResponse.Add(productoModel);
                }*/
                return Json(new
                {
                    success = true,
                    param.sEcho,
                    iTotalRecords = dataResponse.Count(),
                    iTotalDisplayRecords = dataResponse.Count(),//results.Item2,
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
