using MVC_Project.WebBackend.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MVC_Project.WebBackend.Controllers
{
    public class InOutController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetAll(JQueryDataTableParams param)
        {
            try
            {
                List<InOutViewModel> dataResponse = new List<InOutViewModel>();
                for (int i = 0; i < 5; i++)
                {
                    InOutViewModel cediVM = new InOutViewModel
                    {
                        Id = Guid.NewGuid().ToString(),
                        Code = $"ITEM #{i}",
                        Cedis = $"CEDIS #{i}",
                    };
                    dataResponse.Add(cediVM);
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
                    Cedis = "CEDIS 01",
                    Description = "Descripción"
                });
            }
            TempData["ImportResult"] = importResult;
            return RedirectToAction("Import");
        }
    }
}