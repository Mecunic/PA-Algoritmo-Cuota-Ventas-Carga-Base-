using MVC_Project.WebBackend.CustomAttributes.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace MVC_Project.WebBackend.Models
{
    public class PredefinedListItemViewModel
    {
        public int Id { get; set; }
        public string Cedis { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public bool Status { get; set; }
    }

    public class ImportPredefinedListViewModel
    {
        [Display(Name = "CEDIS")]
        [Required]
        public int Cedis { get; set; }

        [Display(Name = "Fecha Inicio")]
        [Required]
        public DateTime? StartDate { get; set; }

        [Display(Name = "Fecha Fin")]
        [Required]
        public DateTime? EndDate { get; set; }

        [Display(Name = "Archivo")]
        public HttpPostedFileBase ExcelFile { get; set; }

        public IEnumerable<PredefinedListProductViewModel> ImportedProducts { get; set; } = new List<PredefinedListProductViewModel>();

        public IEnumerable<SelectListItem> CedisList { get; set; }
    }

    public class PredefinedListProductViewModel
    {
        public int Id { get; set; }

        [Display(Name = "SKU")]
        [Required]
        public string Sku { get; set; }

        [Display(Name = "Producto")]
        public string Product { get; set; }

        [Display(Name = "Cantidad")]
        [Required]
        [MinValue(1, ErrorMessage = "El valor de {0} debe ser mínimo de {1}")]
        public int Amount { get; set; }

        [Display(Name = "Estratégico")]
        public bool IsStrategic { get; set; }

        [Display(Name = "Prioritario")]
        public bool IsPrioritary { get; set; }

        [Display(Name = "Táctico")]
        public bool IsTactic { get; set; }
    }

    public class DetailPredefinedListViewModel
    {
        public int Id { get; set; }
        public string Cedis { get; set; }
    }

    public class CreatePredefinedListViewModel
    {
        [Display(Name = "CEDIS")]
        [Required]
        public int Cedis { get; set; }

        [Display(Name = "Fecha Inicio")]
        [Required]
        public DateTime? StartDate { get; set; }

        [Display(Name = "Fecha Fin")]
        [Required]
        public DateTime? EndDate { get; set; }

        public IEnumerable<SelectListItem> Products { get; set; }
        public IEnumerable<SelectListItem> CedisList { get; set; }

        [Display(Name = "Listado de productos")]
        [Required]
        public IEnumerable<CreateProductPredefinedListViewModel> ProductsList { get; set; }
    }

    public class CreateProductPredefinedListViewModel
    {
        [Display(Name = "Producto")]
        [Required]
        public string ProductId { get; set; }

        [Display(Name = "Cantidad")]
        [Required]
        [MinValue(1, ErrorMessage = "El valor de {0} debe ser mínimo de {1}")]
        public int Amount { get; set; }

        [Display(Name = "Estratégico")]
        public bool IsStrategic { get; set; }

        [Display(Name = "Prioritario")]
        public bool IsPrioritary { get; set; }

        [Display(Name = "Táctico")]
        public bool IsTactic { get; set; }

        public CreateProductPredefinedListViewModel()
        {
            Amount = 1;
        }
    }
}
