using MVC_Project.Data.Services;
using MVC_Project.Domain.Entities;
using MVC_Project.Domain.Services;
using MVC_Project.FlashMessages;
using MVC_Project.WebBackend.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Project.WebBackend.Controllers
{
    public class ParametrosController : BaseController
    {
        private IParametroService _parametroService;
        
        public ParametrosController(IParametroService parametroService)
        {
            _parametroService = parametroService;
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
                var results = _parametroService.FilterBy(filtersValues, param.iDisplayStart, param.iDisplayLength);
                IList<ParametroModel> dataResponse = new List<ParametroModel>();
                foreach (var prod in results.Item1)
                {
                    ParametroModel parametroModel = new ParametroModel
                    {
                        Clave = prod.Clave,
                        Nombre = prod.Nombre,
                        Valor = prod.Valor,
                        Tipo = prod.Tipo,
                        Uuid = prod.Uuid,
                        Estatus = prod.Estatus,
                        AlgoritmoUso = prod.AlgoritmoUso
                    };
                    dataResponse.Add(parametroModel);
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

        public ActionResult Create(string Uuid = null)
        {
            var parametroSaveModel = new ParametroSaveModel();
            if (Uuid != null)
            {
                var parametro = _parametroService.FindBy(p => p.Uuid == Uuid).FirstOrDefault();
                if (parametro != null)
                {
                    parametroSaveModel.Uuid = parametro.Uuid;
                    parametroSaveModel.Clave = parametro.Clave;
                    parametroSaveModel.Nombre = parametro.Nombre;
                    parametroSaveModel.Estatus = parametro.Estatus;
                    parametroSaveModel.Valor = parametro.Valor;
                    parametroSaveModel.Tipo = parametro.Tipo;
                    parametroSaveModel.AlgoritmoUso = parametro.AlgoritmoUso;
                }
            }
            return View(parametroSaveModel);
        }

        [HttpPost, ValidateAntiForgeryToken, ValidateInput(true)]
        public ActionResult Create(ParametroSaveModel model)
        {
            var cultureInfo = new CultureInfo("es-MX");
            ValidacionesAdicionales(model);
            if (ModelState.IsValid)
            {
                if (model.IsNew)
                {
                    var parametro = new Parametro
                    {
                        Clave = model.Clave,
                        Tipo = model.Tipo,
                        Nombre = model.Nombre,
                        Valor = model.Valor,
                        AlgoritmoUso = model.AlgoritmoUso,
                        Estatus = true,
                        FechaModificacion = DateTime.Now,
                        FechaAlta = DateTime.Now,
                        Uuid = Guid.NewGuid().ToString()
                };
                    _parametroService.Create(parametro);
                    MensajeFlashHandler.RegistrarMensaje("Parámetro Creado.", TiposMensaje.Success);
                }
                else
                {
                    
                    var parametro = _parametroService.FindBy(p => p.Uuid == model.Uuid).FirstOrDefault();
                    parametro.Clave = model.Clave;
                    parametro.Tipo = model.Tipo;
                    parametro.Nombre = model.Nombre;
                    parametro.Valor = model.Valor;
                    parametro.AlgoritmoUso = model.AlgoritmoUso;
                    bool estatusActual = parametro.Estatus;
                    parametro.Estatus = model.Estatus;
                    parametro.FechaModificacion = DateTime.Now;
                    if (estatusActual && !parametro.Estatus)
                    {
                        parametro.FechaBaja = DateTime.Now;
                    }
                    else if (!estatusActual && parametro.Estatus)
                    {
                        parametro.FechaBaja = null;
                    }
                    _parametroService.Update(parametro);
                    MensajeFlashHandler.RegistrarMensaje("Parámetro Actualizado.", TiposMensaje.Success);
                    
                }

                return RedirectToAction("Index");
            }
            return View(model);

        }

        [HttpPost]
        public ActionResult Delete(string uuid)
        {
            try
            {
                var parametro = _parametroService.FindBy(x => x.Uuid == uuid).First();
                if (parametro != null)
                {
                    parametro.Estatus = false;
                    parametro.FechaBaja = DateTime.Now;
                    _parametroService.Update(parametro);
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        private void ValidacionesAdicionales(ParametroSaveModel model)
        {
            if (model.IsNew)
            {
                var productoExists = _parametroService.FindBy(p => p.Clave == model.Clave).FirstOrDefault();
                if (productoExists != null)
                {
                    ModelState.AddModelError("Clave", "La Clave ya existe.");
                }
            }
            else
            {
                var productoExists = _parametroService.FindBy(p => p.Clave == model.Clave && p.Uuid != model.Uuid).FirstOrDefault();
                if (productoExists != null)
                {
                    ModelState.AddModelError("Clave", "La Clave ya existe.");
                }
            }
        }

    }
}
