using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace MVC_Project.WebBackend.Models
{
    public class InOutViewModel
    {
        [Display(Name = "Id")]
        public string Id { get; set; }

        [Display(Name = "Clave")]
        public string Code { get; set; }

        [Display(Name = "CEDIS")]
        public string Cedis { get; set; }
    }

    public class ImportInOutViewModel
    {
        [Display(Name = "CEDIS")]
        [Required]
        public string Cedis { get; set; }

        [Display(Name = "Archivo")]
        [Required]
        public HttpPostedFileBase ExcelFile { get; set; }

        public List<ImportedProductViewModel> ImportedProducts { get; set; } = new List<ImportedProductViewModel>();
    }

    public class ImportedProductViewModel
    {
        [Display(Name = "SKU")]
        [Required]
        public string Sku { get; set; }

        [Display(Name = "Descripción")]
        [Required]
        public string Description { get; set; }

        [Display(Name = "CEDIS")]
        public string Cedis { get; set; }
    }

    public class DetailInOutViewModel
    {
        [Display(Name = "Id")]
        public string Id { get; set; }

        [Display(Name = "Clave")]
        public string Code { get; set; }

        [Display(Name = "CEDIS")]
        public string Cedis { get; set; }
    }
}
