using Newtonsoft.Json;
using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Project.WebApis.Modelos
{
    public class TokenResp
    {
        public string error { get; set; }
        public string error_description { get; set; }
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string userId { get; set; }
        [JsonProperty(".issued")]
        public string issued {get;set;}
        [JsonProperty(".expires")]
        public string expires { get; set; }

    }
}
