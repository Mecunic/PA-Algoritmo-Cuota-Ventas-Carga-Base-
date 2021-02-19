using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Project.MotorCalculos.Modelos
{
    public class Inventario
    {
        public string InventarioId { get; set; }
        public string Ruta { get; set; }
        public string Fecha { get; set; }
        public List<ProductoInventario> Productos { get; set; }
        public List<Producto> Reemplazos { get; set; }
        public List<Producto> Implementos { get; set; }
    }

    public class Producto
    {
        public int SKU { get; set; }
        public string Nombre { get; set; }
        public int Cantidad { get; set; }

    }

    public class ProductoInventario : Producto
    {
        public decimal Precio { get; set; }
        public decimal Descuento { get; set; }
        public decimal PorcentajeIva { get; set; }

    }
}
