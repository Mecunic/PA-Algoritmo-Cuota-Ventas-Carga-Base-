using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Project.MotorCalculos.Modelos
{
    public class Venta
    {
        public string VentaId { get; set; }
        public string Ruta { get; set; }
        public string Fecha { get; set; }
        public List<ProductoVenta> Productos { get; set; }
    }

    public class ProductoVenta : Producto
    {
        public decimal Precio { get; set; }
        public decimal Descuento { get; set; }
    }
}
