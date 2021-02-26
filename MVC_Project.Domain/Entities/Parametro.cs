using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Project.Domain.Entities
{
    public class Parametro : IEntity
    {
        public virtual int Id { get; set; }
        public virtual string Uuid { get; set; }
        public virtual string Clave { get; set; }
        public virtual string Nombre { get; set; }
        public virtual string Valor { get; set; }
        public virtual string Tipo { get; set; }
        public virtual string AlgoritmoUso { get; set; }
        public virtual DateTime FechaAlta { get; set; }
        public virtual DateTime FechaModificacion { get; set; }
        public virtual DateTime? FechaBaja { get; set; }
        public virtual bool Estatus { get; set; }
    }
}
