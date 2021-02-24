using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace MVC_Project.WebBackend.Models
{
    public class PredefinedListItemViewModel
    {
        public string Id { get; set; }
        public string Cedis { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }

    public class ImportPredefinedListViewModel
    {
        [Display(Name = "CEDIS")]
        [Required]
        public string Cedis { get; set; }

        [Display(Name = "Fecha Inicio")]
        [Required]
        public DateTime? StartDate { get; set; }

        [Display(Name = "Fecha Fin")]
        [Required]
        public DateTime? EndDate { get; set; }

        [Display(Name = "Archivo")]
        [Required]
        public HttpPostedFileBase ExcelFile { get; set; }

        public List<PredefinedListProductViewModel> ImportedProducts { get; set; } = new List<PredefinedListProductViewModel>();
    }

    public class PredefinedListProductViewModel
    {
        public string Id { get; set; }

        [Display(Name = "SKU")]
        public string Sku { get; set; }

        [Display(Name = "Producto")]
        public string Product { get; set; }

        [Display(Name = "Cantidad")]
        [Required]
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
        public string Id { get; set; }
        public string Cedis { get; set; }
    }

    public class CreatePredefinedListViewModel
    {
        [Display(Name = "CEDIS")]
        [Required]
        public string Cedis { get; set; }

        [Display(Name = "Fecha Inicio")]
        [Required]
        public DateTime? StartDate { get; set; }

        [Display(Name = "Fecha Fin")]
        [Required]
        public DateTime? EndDate { get; set; }

        public IEnumerable<SelectListItem> Products { get; set; }

        public IEnumerable<CreateProductPredefinedListViewModel> ListElement { get; set; }
    }

    public class CreateProductPredefinedListViewModel
    {
        [Display(Name = "Producto")]
        [Required]
        public string ProductId { get; set; }

        [Display(Name = "Cantidad")]
        [Required]
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
