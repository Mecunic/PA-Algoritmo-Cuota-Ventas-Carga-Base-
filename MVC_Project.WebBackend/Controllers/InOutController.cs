using MVC_Project.FlashMessages;
using MVC_Project.Utils;
using MVC_Project.WebBackend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public ActionResult ImportProducts(ImportInOutViewModel model)
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

        [HttpGet]
        public ActionResult Create()
        {
            var model = new CreateInOutViewModel
            {
                AvailableCedis = GetAvailableCedis(),
                AvailableRoutes = GetAvailableRoutes(),
                AvailableProducts = GetAvailableProducts()
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(CreateInOutViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //TODO Guardado en base de datos
                    MensajeFlashHandler.RegistrarMensaje("Lista guardada correctamente.", TiposMensaje.Success);
                    return RedirectToAction("Index");
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
            model.AvailableProducts = GetAvailableProducts();
            model.AvailableCedis = GetAvailableCedis();
            model.AvailableRoutes = GetAvailableRoutes();
            return View(model);
        }

        private List<SelectListItem> GetAvailableProducts()
        {
            //TODO Obtener productos desde el servicio
            return new List<SelectListItem>
                {
                    new SelectListItem
                    {
                        Text = "Producto 1",
                        Value = Guid.NewGuid().ToString()
                    },
                    new SelectListItem
                    {
                        Text = "Producto 2",
                        Value = Guid.NewGuid().ToString()
                    }
                };
        }

        private List<SelectListItem> GetAvailableCedis()
        {
            //TODO Obtener CEDIS desde el servicio
            return new List<SelectListItem>
                {
                    new SelectListItem
                    {
                        Text = "CEDIS 1",
                        Value = Guid.NewGuid().ToString()
                    },
                    new SelectListItem
                    {
                        Text = "CEDIS 2",
                        Value = Guid.NewGuid().ToString()
                    }
                };
        }

        private List<SelectListItem> GetAvailableRoutes()
        {
            //TODO Obtener rutas desde el servicio
            return new List<SelectListItem>
                {
                    new SelectListItem
                    {
                        Text = "Ruta 1",
                        Value = Guid.NewGuid().ToString()
                    },
                    new SelectListItem
                    {
                        Text = "Ruta 2",
                        Value = Guid.NewGuid().ToString()
                    }
                };
        }
    }
}