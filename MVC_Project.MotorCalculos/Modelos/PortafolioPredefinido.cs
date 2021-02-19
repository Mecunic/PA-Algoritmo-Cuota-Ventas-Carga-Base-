using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Project.MotorCalculos.Modelos
{
    public class ProductoPortafolioPredefinido
    {
        public string Marca { get; set; }
        public int SKU { get; set; }
        public string Nombre { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Importe { get; set; }
        public DateTime InicioVigencia { get; set; }
        public DateTime FinVigencia { get; set; }
        public string Pilar { get; set; }
        public string TipoSKU { get; set; }
        public int NumeroPiezas { get; set; }
        public string Ruta { get; set; }
    }
}
