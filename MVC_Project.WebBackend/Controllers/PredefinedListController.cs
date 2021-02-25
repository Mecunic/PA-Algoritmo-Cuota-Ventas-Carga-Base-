using ExcelDataReader;
using MVC_Project.Domain.Entities;
using MVC_Project.Domain.Services;
using MVC_Project.FlashMessages;
using MVC_Project.Utils;
using MVC_Project.WebBackend.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Project.WebBackend.Controllers
{
    public class PredefinedListController : Controller
    {
        private IProductoService _productoService;
        private ICedisService _cedisService;
        private IListaPredefinidaService _listaPredefinidaService;
        private IDetallefinidaService _detallefinidaService;

        public PredefinedListController(IProductoService productoService, ICedisService cedisService,
            IListaPredefinidaService listaPredefinidaService, IDetallefinidaService detallefinidaService)
        {
            _productoService = productoService;
            _cedisService = cedisService;
            _listaPredefinidaService = listaPredefinidaService;
            _detallefinidaService = detallefinidaService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetAll(JQueryDataTableParams param, string filtros = "")
        {
            try
            {
                //List<PredefinedListItemViewModel> dataResponse = new List<PredefinedListItemViewModel>();
                //for (int i = 0; i < 5; i++)
                //{
                //    PredefinedListItemViewModel cediVM = new PredefinedListItemViewModel
                //    {
                //        Id = Guid.NewGuid().ToString(),
                //        Cedis = $"CEDIS #{i}",
                //        StartDate = DateTime.Now.ToString("dd/MM/yyyy"),
                //        EndDate = DateTime.Now.ToString("dd/MM/yyyy"),
                //    };
                //    dataResponse.Add(cediVM);
                //}

                NameValueCollection filtersValues = HttpUtility.ParseQueryString(filtros);
                var results = _listaPredefinidaService.FilterBy(filtersValues, param.iDisplayStart, param.iDisplayLength);

                List<PredefinedListItemViewModel> dataResponse = results.Item1.Select(x => new PredefinedListItemViewModel
                {
                    Id = x.Id,
                    Cedis = x.Cedis.Nombre,
                    StartDate = x.FechaInicio.ToString("dd/MM/yyyy"),
                    EndDate = x.FechaFin.ToString("dd/MM/yyyy")
                }).ToList();

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

        [Route("/Detail/{id}")]
        public ActionResult Detail(int id)
        {
            var result = _listaPredefinidaService.GetById(id);

            if (result != null)
            {
                var model = new DetailPredefinedListViewModel
                {
                    Id = id,
                    Cedis = result.Cedis.Nombre
                };
                return View(model);
            }

            return new HttpNotFoundResult("El elemento seleccionado no fue encontrado.");
        }

        [Route("/GetDetailProducts/{id}")]
        public JsonResult GetDetailProducts(JQueryDataTableParams param, int id)
        {
            try
            {
                NameValueCollection filtersValues = HttpUtility.ParseQueryString("");
                var results = _detallefinidaService.FilterBy(id, filtersValues, param.iDisplayStart, param.iDisplayLength);

                List<PredefinedListProductViewModel> dataResponse = results.Item1.Select(x => new PredefinedListProductViewModel
                {
                    Id = x.Id,
                    Sku = x.Producto.SKU,
                    Product = x.Producto.Descripcion,
                    Amount = x.Cantidad,
                    IsPrioritary = x.Prioritario,
                    IsStrategic = x.Estrategico,
                    IsTactic = x.Tactico
                }).ToList();

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
                        Value = Guid.NewGuid().ToString()
                    },
                    new SelectListItem
                    {
                        Text = "Producto 2",
                        Value = Guid.NewGuid().ToString()
                    }
                }
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(CreatePredefinedListViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //save
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

            return View(model);
        }

        [HttpGet]
        public ActionResult Import()
        {
            var cedis = _cedisService.GetAll();

            var importResult = (ImportPredefinedListViewModel)TempData["ImportResult"];
            var model = importResult ?? new ImportPredefinedListViewModel();

            model.CedisList = cedis.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Nombre });

            return View(model);
        }

        [HttpPost]
        public ActionResult ProductsImporter(ImportPredefinedListViewModel model)
        {
            //model.ImportedProducts = new List<PredefinedListProductViewModel>();

            if (ModelState.IsValid)
            {
                try
                {
                    if (model.ExcelFile != null)
                    {
                        model.ImportedProducts = GetDetailsFromFile(model.ExcelFile);
                    }
                    else
                    {
                        var now = DateUtil.GetDateTimeNow();
                        ListaPredefinida listaPredefinida = new ListaPredefinida
                        {
                            Cedis = new Cedis { Id = model.Cedis },
                            FechaInicio = model.StartDate.GetValueOrDefault(),
                            FechaFin = model.EndDate.GetValueOrDefault(),
                            FechaAlta = now,
                            FechaModificacion = now,
                            Estatus = true,
                        };

                        List<DetallePredefinida> detallePredefinidas = model.ImportedProducts.Select(x => new DetallePredefinida
                        {
                            Producto = new Producto { IdProducto = x.Sku },
                            ListaPredefinida = listaPredefinida,
                            Cantidad = x.Amount,
                            Estrategico = x.IsStrategic,
                            Prioritario = x.IsPrioritary,
                            Tactico = x.IsTactic,
                            FechaAlta = now,
                            FechaModificacion = now,
                            Estatus = true
                        }).ToList();

                        listaPredefinida.DetallesPredefinida = detallePredefinidas;

                        _listaPredefinidaService.Create(listaPredefinida);

                        MensajeFlashHandler.RegistrarMensaje("Lista guardada correctamente.", TiposMensaje.Success);
                        return RedirectToAction("Index");
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

        private List<PredefinedListProductViewModel> GetDetailsFromFile(HttpPostedFileBase File)
        {
            List<PredefinedListProductViewModel> items = new List<PredefinedListProductViewModel>();
            using (var stream = File.InputStream)
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

                            items.Add(new PredefinedListProductViewModel
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

            return items;
        }
    }
}