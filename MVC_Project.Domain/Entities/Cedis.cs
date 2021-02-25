using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Project.Domain.Entities
{
    public class Cedis : IEntity
    {
        public virtual int Id { get; set; }
        public virtual string Clave { get; set; }
        public virtual string Nombre { get; set; }
        public virtual string Descripcion { get; set; }
        public virtual DateTime FechaAlta { get; set; }
        public virtual DateTime FechaModificacion { get; set; }
        public virtual User Responsable { get; set; }
        public virtual IList<User> Usuarios { get; set; }

        public Cedis()
        {
            Usuarios = new List<User>();
        }
    }
}
