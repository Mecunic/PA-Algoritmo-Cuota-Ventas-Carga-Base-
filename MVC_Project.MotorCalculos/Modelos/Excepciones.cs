using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Project.MotorCalculos.Modelos
{
    public class CargaException : Exception
    {
        public CargaException()
        {
        }

        public CargaException(string message)
            : base(message)
        {
        }

        public CargaException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
