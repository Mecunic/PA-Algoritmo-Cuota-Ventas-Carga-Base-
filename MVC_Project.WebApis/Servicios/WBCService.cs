using MVC_Project.WebApis.Modelos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Project.WebApis.Servicios
{
    public class WBCService : BaseService
    {
        const string WEB_API = "WEB_API_WBC";
        const string GRANT_TYPE = "GRANT_TYPE";
        const string USERNAME = "USERNAME";
        const string PASSWORD = "PASSWORD";
        /// <summary>
        /// Este token vence el día Thu, 25 Mar 2021 17:49:14 GMT USAR CUANTO SEA POSIBLE NO GENERAR UNO DE NUEVO
        /// </summary>
        const string TOKEN = "z-sRdyxSrYa_dRYy8yCEFDuynrMIVu72EaSlSFWWCuUdbbU4H3dVvuY2H1Fo7TSiua5jbJBU-2D7cTL0wf33QeI_IGKuIy1a-0KLW-bWZkYYOb-A9osyaVe37D93tZ_Jk_fOayMWWhVqqUHCKoklQzrKUazqFx2gm1zYT7_PV8kutVBiMuBMzFsgnt8lDA4J345v6j0gdEjwX9C8KOjW4EG_QFo8jZyeeY29-xrRe-qoNjZg6IfyCtnE7A3vejUYD8puyIxrdvVJTYejnA-qFmy0p6hPkjf8w39ib-MwbVRGhsEDbaZDnCTLX70Bd3K05ARZ-idN0IapHxiZKa29vYQuusBCr4_0-DQ5zsc_d9J8wQqr_XY5Luq3P-5N9shs";

        public static TokenResp Token()
        {
            TokenReq req = new TokenReq();
            req.grant_type = AppSettings(GRANT_TYPE);
            req.username = AppSettings(GRANT_TYPE);
            req.password = AppSettings(GRANT_TYPE);
            //var response = CallServiceUrlEncoder<TokenResp, TokenReq>(AppSettings(WEB_API), "/token", RestSharp.Method.POST, req);
            string json = "{\"access_token\":\"z-sRdyxSrYa_dRYy8yCEFDuynrMIVu72EaSlSFWWCuUdbbU4H3dVvuY2H1Fo7TSiua5jbJBU-2D7cTL0wf33QeI_IGKuIy1a-0KLW-bWZkYYOb-A9osyaVe37D93tZ_Jk_fOayMWWhVqqUHCKoklQzrKUazqFx2gm1zYT7_PV8kutVBiMuBMzFsgnt8lDA4J345v6j0gdEjwX9C8KOjW4EG_QFo8jZyeeY29-xrRe-qoNjZg6IfyCtnE7A3vejUYD8puyIxrdvVJTYejnA-qFmy0p6hPkjf8w39ib-MwbVRGhsEDbaZDnCTLX70Bd3K05ARZ-idN0IapHxiZKa29vYQuusBCr4_0-DQ5zsc_d9J8wQqr_XY5Luq3P-5N9shs\",\"token_type\":\"bearer\",\"expires_in\":1209599,\"userId\":\"2777\",\"userName\":\"user_integracion\",\".issued\":\"Thu, 11 Mar 2021 17:49:14 GMT\",\".expires\":\"Thu, 25 Mar 2021 17:49:14 GMT\"}";
            var resp = Newtonsoft.Json.JsonConvert.DeserializeObject<TokenResp>(json);
            return resp;
        }
    }
}
