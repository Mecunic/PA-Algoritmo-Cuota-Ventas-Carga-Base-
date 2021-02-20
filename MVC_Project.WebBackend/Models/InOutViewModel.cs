using System;
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
        public string Cedis { get; set; }

        [Display(Name = "Archivo")]
        public HttpPostedFile ExcelFile { get; set; }
    }
}
