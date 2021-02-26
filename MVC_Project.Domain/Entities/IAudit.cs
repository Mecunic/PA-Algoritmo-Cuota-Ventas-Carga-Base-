using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Project.Domain.Entities
{
    public interface IAudit
    {
        DateTime FechaAlta { get; set; }
        DateTime FechaModificacion { get; set; }
        DateTime FechaBaja { get; set; }
    }
}
