using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Project.WebApis.Modelos
{
    public class CedisResp
    {
        [JsonProperty("Id")]
        public int Id { get; set; }
        [JsonProperty("CedisNameCCHT")]
        public string NombreCocaColaHogar { get; set; }
        [JsonProperty("CedisIdOpecd")]
        public int CedisId { get; set; }
        [JsonProperty("Nombre")]
        public string nombre { get; set; }
    }
}
