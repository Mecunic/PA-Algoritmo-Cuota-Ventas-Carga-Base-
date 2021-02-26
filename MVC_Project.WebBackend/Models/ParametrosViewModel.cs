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
        public string Parametro { get; set; }
        public string Valor { get; set; }
        public string Tipo { get; set; }
        public string AlgoritmoUso { get; set; }
    }

}