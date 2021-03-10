using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Project.MotorCalculos.Modelos
{
    public class ConfiguracionCalculoCarga
    {
        /// <summary>
        ///  Listado de Productos para realizar un calculo de Carga
        /// </summary>
        public List<ProductoPortafolioPredefinido> PortafolioPredefinido { get; set; }
        /// <summary>
        /// Inventario de Productos, este listado debe contener las semanas a calcular
        /// </summary>
        public List<Inventario> Cargas { get; set; }
        /// <summary>
        /// Historico de Ventas, este listado puede no contener ventas de ciertos productos en ciertas fechas (Datawarehouse)
        /// </summary>
        public List<Venta> Ventas { get; set; }
        /// <summary>
        /// Ruta para filtrar el inventario y las ventas
        /// </summary>
        public String Ruta { get; set; }
        /// <summary>
        /// Fecha proxima a calcular
        /// </summary>
        public String FechaACalcular { get; set; }
        /// <summary>
        /// Numero de Semanas hacia atras a calcular
        /// </summary>
        public int Semanas { get; set; }
    }
}
