using MVC_Project.Domain.Entities;
using MVC_Project.Domain.Services;
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
    public class ProductosController : BaseController
    {
        private IProductoService _productoService;
        private ITipoEmpaqueService _tipoEmpaqueService;
        private ICategoriaService _categoriaService;
        private IPresentacionService _presentacionService;
        private IUnidadEmpaqueService _unidadEmpaqueService;
        public ProductosController(IProductoService productoService, ITipoEmpaqueService tipoEmpaqueService, ICategoriaService categoriaService, IUnidadEmpaqueService unidadEmpaqueService, IPresentacionService presentacionService)
        {
            _productoService = productoService;
            _tipoEmpaqueService = tipoEmpaqueService;
            _categoriaService = categoriaService;
            _unidadEmpaqueService = unidadEmpaqueService;
            _presentacionService = presentacionService;
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
                var results = _productoService.FilterBy(filtersValues, param.iDisplayStart, param.iDisplayLength);
                IList<ProductoModel> dataResponse = new List<ProductoModel>();
                foreach (var prod in results.Item1)
                {
                    ProductoModel productoModel = new ProductoModel();
                    productoModel.SKU = prod.SKU;
                    productoModel.TipoEmpaque = prod.TipoEmpaque?.Name;
                    productoModel.Presentacion = prod.Presentacion.Name;
                    productoModel.Status = prod.Estatus;
                    productoModel.Descripcion = prod.Descripcion;
                    dataResponse.Add(productoModel);
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

        public ActionResult Create(string uuid = null)
        {
            var productoSaveModel = new ProductoSaveModel
            { Categorias = PopulateCategorias(), Presentaciones = PopulatePresentaciones(), UnidadesEmpaque = PopulateUnidadesEmpaque(), TiposEmpaque = PopulateTiposEmpaque()  };
            if(uuid != null)
            {
                var producto = _productoService.FindBy(p => p.Uuid == uuid).FirstOrDefault();
                if(producto != null)
                {
                    productoSaveModel.Uuid = producto.Id.ToString();
                    productoSaveModel.UnidadEmpaque = producto.UnidadEmpaque.Uuid;
                    productoSaveModel.TipoEmpaque = producto.TipoEmpaque?.Uuid;
                    productoSaveModel.Status = producto.Estatus;
                    productoSaveModel.Presentacion = producto.Presentacion.Uuid;
                    productoSaveModel.SKU = producto.SKU;
                    productoSaveModel.Categoria = producto.Categoria.Uuid;
                    //productoSaveModel.ProductoEstrategico = producto.ProductoEstrategico;
                    productoSaveModel.Descripcion = producto.Descripcion;
                }
            }
            return View(productoSaveModel);
        }

        [HttpPost, ValidateAntiForgeryToken, ValidateInput(true)]
        public ActionResult Create(ProductoSaveModel model)
        {
            var cultureInfo = new CultureInfo("es-MX");
            ValidacionesAdicionales(model);
            if (ModelState.IsValid)
            {
                if (model.IsNew)
                {
                    var producto = new Producto
                    {
                        //Uuid = Guid.NewGuid().ToString(),
                        SKU = model.SKU,
                        Categoria = new Categoria { Uuid = model.Categoria },
                        Descripcion = model.Descripcion,
                        Presentacion = new Presentacion { Uuid = model.Presentacion },
                        //ProductoEstrategico = model.ProductoEstrategico,
                        Estatus = true,
                        TipoEmpaque = new TipoEmpaque { Uuid = model.TipoEmpaque },
                        UnidadEmpaque = new UnidadEmpaque { Uuid = model.UnidadEmpaque }
                    };
                    producto.Categoria = _categoriaService.FindBy(c => c.Uuid == producto.Categoria.Uuid).FirstOrDefault();
                    producto.TipoEmpaque = _tipoEmpaqueService.FindBy(te => te.Uuid == producto.TipoEmpaque.Uuid).FirstOrDefault();
                    producto.UnidadEmpaque = _unidadEmpaqueService.FindBy(ue => ue.Uuid == producto.UnidadEmpaque.Uuid).FirstOrDefault();
                    producto.Presentacion = _presentacionService.FindBy(ps => ps.Uuid == producto.Presentacion.Uuid).FirstOrDefault();
                    _productoService.Create(producto);

                    ViewBag.Message = "Producto Creado";
                }
                else
                {
                    var producto = _productoService.FindBy(p => p.Uuid == model.Uuid).FirstOrDefault();
                    producto.SKU = model.SKU;
                    producto.Categoria = new Categoria { Uuid = model.Categoria };
                    producto.Descripcion = model.Descripcion;
                    producto.Presentacion = new Presentacion { Uuid = model.Presentacion };
                    producto.Estatus = model.Status;
                    producto.TipoEmpaque = new TipoEmpaque { Uuid = model.TipoEmpaque };
                    producto.UnidadEmpaque = new UnidadEmpaque { Uuid = model.UnidadEmpaque };
                    producto.Categoria = _categoriaService.FindBy(c => c.Uuid == producto.Categoria.Uuid).FirstOrDefault();
                    producto.TipoEmpaque = _tipoEmpaqueService.FindBy(te => te.Uuid == producto.TipoEmpaque.Uuid).FirstOrDefault();
                    producto.UnidadEmpaque = _unidadEmpaqueService.FindBy(ue => ue.Uuid == producto.UnidadEmpaque.Uuid).FirstOrDefault();
                    producto.Presentacion = _presentacionService.FindBy(ps => ps.Uuid == producto.Presentacion.Uuid).FirstOrDefault();
                    _productoService.Update(producto);

                    ViewBag.Message = "Producto Actualizado";
                    
                }
                
                return View("Index");
            }
            else
            {
                model.Categorias = PopulateCategorias();
                model.Presentaciones = PopulatePresentaciones();
                model.UnidadesEmpaque = PopulateUnidadesEmpaque();
                model.TiposEmpaque = PopulateTiposEmpaque();
                return View(model);
            }

        }

        private IEnumerable<SelectListItem> PopulateTiposEmpaque()
        {
            var availableTiposEmpaque = _tipoEmpaqueService.GetAll().Where(c => c.Status == true); ;
            var tiposEmpaqueList = new List<SelectListItem>();
            tiposEmpaqueList = availableTiposEmpaque.Select(tipo => new SelectListItem
            {
                Value = tipo.Uuid,
                Text = tipo.Name
            }).ToList();
            return tiposEmpaqueList;
        }

        private IEnumerable<SelectListItem> PopulateUnidadesEmpaque()
        {
            var availableUnidades = _unidadEmpaqueService.GetAll().Where(c => c.Status == true); ;
            var unidadesList = new List<SelectListItem>();
            unidadesList = availableUnidades.Select(unidad => new SelectListItem
            {
                Value = unidad.Uuid,
                Text = unidad.Name
            }).ToList();
            return unidadesList;
        }
        private IEnumerable<SelectListItem> PopulateCategorias()
        {
            var availableCategorias = _categoriaService.GetAll().Where(c => c.Status == true); ;
            var categoriasList = new List<SelectListItem>();
            categoriasList = availableCategorias.Select(categorias => new SelectListItem
            {
                Value = categorias.Uuid,
                Text = categorias.Name
            }).ToList();
            return categoriasList;
        }

        private IEnumerable<SelectListItem> PopulatePresentaciones()
        {
            var availablePresentaciones = _presentacionService.GetAll().Where(c => c.Status == true); ;
            var rolesList = new List<SelectListItem>();
            rolesList = availablePresentaciones.Select(presentacion => new SelectListItem
            {
                Value = presentacion.Uuid,
                Text = presentacion.Name
            }).ToList();
            return rolesList;
        }

        private void ValidacionesAdicionales(ProductoSaveModel model)
        {
            if (model.IsNew)
            {
                var productoExists = _productoService.FindBy(p => p.SKU == model.SKU).FirstOrDefault();
                if(productoExists != null)
                {
                    ModelState.AddModelError("SKU", "El SKU ya existe.");
                }
            }
            else
            {
                var productoExists = _productoService.FindBy(p => p.SKU == model.SKU).FirstOrDefault();
                if (productoExists != null)
                {
                    ModelState.AddModelError("SKU", "El SKU ya existe.");
                }
            }
        }

    }
}
