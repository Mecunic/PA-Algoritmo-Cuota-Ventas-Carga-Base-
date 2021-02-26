using MVC_Project.WebBackend.Models.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC_Project.WebBackend.Models
{
    public class ParametrosViewModel : DataTableModel
    {
        [Display(Name = "Filtro")]
        public string Filtro { get; set; }
    }
    
    public class ParametroModel
    {
        public string Clave { get; set; }
        public string Nombre { get; set; }
        public string Valor { get; set; }
        public string Tipo { get; set; }
        public string AlgoritmoUso { get; set; }
        public string Uuid { get; set; }
        public bool Estatus { get; set; }
    }

    public class ParametroSaveModel
    {
        [Required]
        [ValidType(CustomAttributes.Validations.ValidType.WITHOUT_SPACES)]
        public string Clave { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Valor { get; set; }
        [Required]
        public string Tipo { get; set; }
        [Required]
        public string AlgoritmoUso { get; set; }
        public string Uuid { get; set; }
        public bool Estatus { get; set; }
        public bool IsNew
        {
            get
            {
                return !(Uuid != null && Uuid.Trim().Length > 0);
            }
        }
    }

}