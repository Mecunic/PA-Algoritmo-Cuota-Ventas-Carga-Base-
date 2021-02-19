using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Project.MotorCalculos.Modelos
{
    public class ConfiguracionCalculoCarga
    {
        public List<ProductoPortafolioPredefinido> PortafolioPredefinido { get; set; }
        public List<Inventario> Inventarios { get; set; }
        public String Ruta { get; set; }
        public String FechaACalcular { get; set; }
        public int Semanas { get; set; }
    }
}
