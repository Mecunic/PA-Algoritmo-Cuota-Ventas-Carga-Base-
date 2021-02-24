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
                        Cedis = $"CEDIS #{i}",
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

        [Route("/Detail/{id}")]
        public ActionResult Detail(string id)
        {
            var model = new DetailPredefinedListViewModel
            {
                Id = id,
                Cedis = "Cedis"
            };
            return View(model);
        }

        [Route("/GetDetailProducts/{id}")]
        public JsonResult GetDetailProducts(JQueryDataTableParams param, string id)
        {
            try
            {
                List<PredefinedListProductViewModel> dataResponse = new List<PredefinedListProductViewModel>();
                for (int i = 0; i < 5; i++)
                {
                    PredefinedListProductViewModel listItem = new PredefinedListProductViewModel
                    {
                        Id = Guid.NewGuid().ToString(),
                        Sku = $"SKU #{i}",
                        Product = "Producto",
                        Amount = 10,
                        IsPrioritary = true,
                        IsStrategic = false,
                        IsTactic = true
                    };
                    dataResponse.Add(listItem);
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
        public ActionResult Create()
        {
            var model = new CreatePredefinedListViewModel
            {
                Products = new List<SelectListItem>
                {
                    new SelectListItem
                    {
                        Text = "Producto 1",
                        Value = "1"
                    },
                    new SelectListItem
                    {
                        Text = "Producto 2",
                        Value = "2"
                    }
                }
            };

            return View(model);
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
                                    string product = Convert.ToString(row[1]);
                                    int amount = Convert.ToInt32(row[2]);
                                    string isStrategicTextVal = Convert.ToString(row[3]).ToLower();
                                    string isPrioritaryTextVal = Convert.ToString(row[4]).ToLower();
                                    string isTacticTextVal = Convert.ToString(row[5]).ToLower();

                                    bool isStategic = isStrategicTextVal == "sí" || isStrategicTextVal == "si";
                                    bool isPrioritary = isPrioritaryTextVal == "sí" || isPrioritaryTextVal == "si";
                                    bool isTactic = isTacticTextVal == "sí" || isTacticTextVal == "si";

                                    model.ImportedProducts.Add(new PredefinedListProductViewModel
                                    {
                                        Sku = sku,
                                        Product = product,
                                        Amount = amount,
                                        IsStrategic = isStategic,
                                        IsPrioritary = isPrioritary,
                                        IsTactic = isTactic
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