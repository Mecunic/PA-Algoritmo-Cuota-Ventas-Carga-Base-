using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Project.WebApis.Modelos
{
    public class ProductoResp
    {
        public int Id { get; set; }
        public string CodigoProducto { get; set; }
        public string Nombre { get; set; }
        public int Ruta { get; set; }
        public int Cedis { get; set; }
        public string Status { get; set; }
        public int WBCId { get; set; }
        public int Tipo { get; set; }
        public string CreateDate { get; set; }
    }
}
