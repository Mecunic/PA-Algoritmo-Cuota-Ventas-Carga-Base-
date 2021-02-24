using MVC_Project.Utils;
using MVC_Project.WebBackend.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MVC_Project.WebBackend.Controllers
{
    public class InOutController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetAll(JQueryDataTableParams param)
        {
            try
            {
                List<InOutViewModel> dataResponse = new List<InOutViewModel>();
                for (int i = 0; i < 5; i++)
                {
                    InOutViewModel listVM = new InOutViewModel
                    {
                        Id = Guid.NewGuid().ToString(),
                        Cedis = $"CEDIS #{i}",
                        Route = $"RUTA #{i}",
                        StartDate = DateUtil.GetDateTimeNow().ToString("dd/MM/yyyy"),
                        EndDate = DateUtil.GetDateTimeNow().AddMonths(15).ToString("dd/MM/yyyy"),
                    };
                    dataResponse.Add(listVM);
                }
                return Json(new
                {
                    success = true,
                    param.sEcho,
                    iTotalRecords = dataResponse.Count,
                    iTotalDisplayRecords = 5,
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

        [HttpGet]
        public ActionResult Import()
        {
            var importResult = (ImportInOutViewModel)TempData["ImportResult"];
            var model = importResult ?? new ImportInOutViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult ProductsImporter(ImportInOutViewModel model)
        {
            var importResult = new ImportInOutViewModel()
            {
                ImportedProducts = new List<ImportedProductViewModel>()
            };
            for (int i = 0; i < 5; i++)
            {
                importResult.ImportedProducts.Add(new ImportedProductViewModel
                {
                    Sku = Guid.NewGuid().ToString(),
                    Name = $"Producto #{i}",
                    Quantity = i + 1
                });
            }
            TempData["ImportResult"] = importResult;
            return RedirectToAction("Import");
        }

        [HttpGet]
        [Route("~/Detail/{id}")]
        public ActionResult Detail(string id)
        {
            InOutViewModel listVM = new InOutViewModel
            {
                Id = Guid.NewGuid().ToString(),
                Cedis = "CEDIS #1",
            };
            return View(listVM);
        }

        [HttpGet]
        [Route("~/GetProducts/{id}")]
        public JsonResult GetProducts(JQueryDataTableParams param, string id)
        {
            try
            {
                List<ImportedProductViewModel> dataResponse = new List<ImportedProductViewModel>();
                for (int i = 0; i < 5; i++)
                {
                    dataResponse.Add(new ImportedProductViewModel
                    {
                        Sku = Guid.NewGuid().ToString(),
                        Name = $"Producto #{i}",
                        Quantity = i + 1
                    });
                }
                return Json(new
                {
                    success = true,
                    param.sEcho,
                    iTotalRecords = dataResponse.Count,
                    iTotalDisplayRecords = 5,
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