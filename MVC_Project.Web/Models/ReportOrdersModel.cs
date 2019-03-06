using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Project.Web.Models
{
    public class ReportOrdersModel
    {
        public string Nombre { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Fin { get; set; }
        public int? Status { get; set; }
        public SelectList ListStatus { get; set; }
        public int? Tienda { get; set; }
        public SelectList ListTiendas { get; set; }
    }
    public class OrdersData
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Estatus { get; set; }
        public string Cliente { get; set; }
        public DateTime ShipperAt { get; set; }
        public string Tienda { get; set; }
        public string Vendedor { get; set; }
    }
}