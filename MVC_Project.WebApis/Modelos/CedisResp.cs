using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Project.WebApis.Modelos
{
    public class CedisResp
    {
        public int Id { get; set; }
        /// <summary>
        /// Nombre Coca Cola Hogar
        /// </summary>
        public string CedisNameCCHT { get; set; }
        /// <summary>
        /// Cedis Id
        /// </summary>
        public int CedisIdOpecd { get; set; }
        /// <summary>
        /// Nombre del Cedis
        /// </summary>
        public string Nombre { get; set; }
    }
}
