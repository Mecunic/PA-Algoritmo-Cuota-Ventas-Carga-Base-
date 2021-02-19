using Microsoft.Analytics.Interfaces;
using Microsoft.Analytics.Interfaces.Streaming;
using Microsoft.Analytics.Types.Sql;
using MVC_Project.MotorCalculos.Modelos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MVC_Project.MotorCalculos
{
    public class Cargas
    {
        public static CargaDiaria CalcularCargaTotal(ConfiguracionCalculoCarga configuracion)
        {
            return new CargaDiaria();
        }
    }
}