using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace MVC_Project.WebBackend.Models
{
    public class PredefinedListItemViewModel
    {
        public string Id { get; set; }
        public string Sku { get; set; }
        public string Description { get; set; }
        public string PackageUnit { get; set; }
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
        [Display(Name = "SKU")]
        public string Sku { get; set; }

        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [Display(Name = "Unid. Empaque")]
        public string PackageUnit { get; set; }

        [Display(Name = "Fecha Inicio")]
        [Required]
        public DateTime? StartDate { get; set; }

        [Display(Name = "Fecha Fin")]
        [Required]
        public DateTime? EndDate { get; set; }

        [Display(Name = "Cantidad")]
        [Required]
        public int Amount { get; set; }
    }
}
