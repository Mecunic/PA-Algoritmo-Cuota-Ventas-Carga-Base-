
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC_Project.WebBackend.Models
{
    public class ProductoViewModel : DataTableModel
    {
        [Display(Name = "Filtro")]
        public string Filtro { get; set; }
        public ProductoModel ProductoModelList { get; set; }
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
    }
}