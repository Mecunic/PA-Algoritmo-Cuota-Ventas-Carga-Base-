using ExcelDataReader;
using MVC_Project.WebBackend.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Project.WebBackend.Controllers
{
    public class PredefinedListController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetAll(JQueryDataTableParams param)
        {
            try
            {
                List<PredefinedListItemViewModel> dataResponse = new List<PredefinedListItemViewModel>();
                for (int i = 0; i < 5; i++)
                {
                    PredefinedListItemViewModel cediVM = new PredefinedListItemViewModel
                    {
                        Id = Guid.NewGuid().ToString(),
                        Sku = $"SKU #{i}",
                        Description = "Descripción",
                        PackageUnit = "Unidad",
                        StartDate = DateTime.Now.ToString("dd/MM/yyyy"),
                        EndDate = DateTime.Now.ToString("dd/MM/yyyy"),
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
            var importResult = (ImportPredefinedListViewModel)TempData["ImportResult"];
            var model = importResult ?? new ImportPredefinedListViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult ProductsImporter(ImportPredefinedListViewModel model)
        {
            model.ImportedProducts = new List<PredefinedListProductViewModel>();

            if (ModelState.IsValid)
            {
                try
                {
                    using (var stream = model.ExcelFile.InputStream)
                    {
                        using (var reader = ExcelReaderFactory.CreateReader(stream))
                        {
                            var result = reader.AsDataSet();
                            var pages = result.Tables;
                            foreach (DataTable page in pages)
                            {
                                for (int i = 1; i < page.Rows.Count; i++)
                                {
                                    DataRow row = page.Rows[i];
                                    string sku = Convert.ToString(row[0]);
                                    string description = Convert.ToString(row[1]);
                                    string packageUnit = Convert.ToString(row[2]);

                                    model.ImportedProducts.Add(new PredefinedListProductViewModel
                                    {
                                        Sku = sku,
                                        Description = description,
                                        PackageUnit = packageUnit,
                                        StartDate = model.StartDate,
                                        EndDate = model.EndDate,
                                    });
                                }
                            }
                            // The result of each spreadsheet is in result.Tables
                        }
                    }
                }
                catch (Exception ex)
                {
                    Session.Add("View.Message", new MessageView
                    {
                        type = TypeMessageView.ERROR,
                        description = ex.Message
                    });
                }
            }
            else
            {
                var errorKey = ModelState.Keys.Where(k => ModelState[k].Errors.Count > 0).FirstOrDefault();
                Session.Add("View.Message", new MessageView
                {
                    type = TypeMessageView.ERROR,
                    description = !string.IsNullOrEmpty(errorKey) ? ModelState[errorKey].Errors[0].ErrorMessage
                        : "Error de validación, verifique los datos e inténtelo de nuevo."
                });
            }

            TempData["ImportResult"] = model;
            return RedirectToAction("Import");
        }
    }
}