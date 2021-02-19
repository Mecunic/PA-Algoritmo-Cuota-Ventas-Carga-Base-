using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Project.MotorCalculos
{
    public class Utils
    {
        public static CultureInfo CULTURE_INFO = new CultureInfo("es-MX");
        public static readonly string FORMAT_DATE  = "yyyy-MM-dd";
        public static List<string> ObtenerSemanasAtras(string fecha,int semanas)
        {
            List<String> fechaDias = new List<string>();
            
            for(int s = 1; s <= semanas; s++)
            {
                DateTime fechaAtras = DateTime.ParseExact(fecha, FORMAT_DATE, CULTURE_INFO);
                fechaAtras.AddDays(s * 7 * -1);
                fechaDias.Add(fechaAtras.ToString(FORMAT_DATE));
            }
            return fechaDias;
        }
    }
}
