using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Project.MotorCalculos.Modelos
{
    public class CargaDiaria
    {
        public string Fecha { get; set; }
        public string Ruta { get; set; }
        public List<Producto> Productos { get; set; }
        public CargaDiaria()
        {
            Productos = new List<Producto>();
        }
    }
}
