using MVC_Project.Domain.Services;
using MVC_Project.WebBackend.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;

namespace MVC_Project.WebBackend.Controllers
{
    public class CedisController : BaseController
    {
        private ICedisService _cedisService;

        public CedisController(ICedisService cedisService)
        {
            _cedisService = cedisService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetAll(JQueryDataTableParams param, string filtros)
        {
            try
            {
                NameValueCollection filtersValues = HttpUtility.ParseQueryString(filtros);
                var results = _cedisService.FilterBy(filtersValues, param.iDisplayStart, param.iDisplayLength);
                List<CedisViewModel> dataResponse = new List<CedisViewModel>();
                foreach (var cedi in results.Item1)
                {
                    CedisViewModel cediVM = new CedisViewModel
                    {
                        Id = cedi.Uuid,
                        Code = cedi.Code,
                        Name = cedi.Name,
                        Description = cedi.Description,
                        Manager = cedi.Manager != null ? $"{cedi.Manager.FirstName} {cedi.Manager.ApellidoPaterno}" : "NA"
                    };
                    dataResponse.Add(cediVM);
                }
                return Json(new
                {
                    success = true,
                    param.sEcho,
                    iTotalRecords = dataResponse.Count,
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
                    MaxJsonLength = int.MaxValue
                };
            }
        }

    }
}
