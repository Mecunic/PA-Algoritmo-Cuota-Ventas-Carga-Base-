using Hangfire;
using MVC_Project.WebApis.Modelos;
using MVC_Project.WebApis.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_Project.Jobs
{
    public class HPagosJob
    {
        [DisableConcurrentExecution(0)] //Se utiliza para tomar un timeout. Fuente: https://www.hangfire.io/blog/2014/05/21/hangfire-0.8.2-released.html
        public static void ListaCedis()
        {
            //Ejemplo de generación de llamado de servicio
            List<CedisResp> listCedis = IntermediaService.Cedis();

            #region
            //EndEventHandler esta sección se puede agregar la logica de obtener el historico de ventas y convertirlo o manipularlo
            #endregion
        }

    }
}