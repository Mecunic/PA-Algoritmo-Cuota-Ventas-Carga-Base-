﻿
using MVC_Project.WebBackend.Models.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Project.WebBackend.Models
{
    public class ProductoViewModel : DataTableModel
    {
        [Display(Name = "Filtro")]
        public string Filtro { get; set; }
        public ParametroModel ProductoModelList { get; set; }
    }
    public class ProductoModel
    {
        public int Id { get; set; }
        public string SKU { get; set; }
        public string Descripcion { get; set; }
        public string TipoEmpaque { get; set; }
        public decimal PrecioUnitario { get; set; }
        public string Presentacion { get; set; }
        public bool Status { get; set; }
        public string Uuid { get; set; }
    }

    public class ProductoSaveModel
    {
        [Required]
        [ValidType(CustomAttributes.Validations.ValidType.NUMERICS)]
        public string SKU { get; set; }
        [Required]
        public string Descripcion { get; set; }

        [Display(Name ="Tipo de Empaque")]
        public string TipoEmpaque { get; set; }
        public IEnumerable<SelectListItem> TiposEmpaque { get; set; }
        [Required]
        [Display(Name = "Unidad por Empaque")]
        public string UnidadEmpaque { get; set; }
        public IEnumerable<SelectListItem> UnidadesEmpaque { get; set; }
        [Display(Name = "Producto Estrategico")]
        public bool ProductoEstrategico { get; set; }
        [Required]
        public string Categoria { get; set; }
        public IEnumerable<SelectListItem> Categorias { get; set; }
        [Required]
        public string Presentacion { get; set; }
        public IEnumerable<SelectListItem> Presentaciones { get; set; }
        [Required]
        [Display(Name = "Precio Unitario")]
        public decimal PrecioUnitario { get; set; }
        [Required]
        [Display(Name = "Precio por Empaque")]
        public decimal PrecioEmpaque { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public string Uuid { get; set; }
        public bool Status { get; set; }
        public bool IsNew
        {
            get
            {
                return !(Uuid != null && Uuid.Trim().Length > 0);
            }
        }
    }
}