using System;
using System.Collections.Generic;

namespace MVC_Project.Domain.Entities
{
    public class ListaPredefinida : IEntity
    {
        public virtual int Id { get; set; }
        public virtual DateTime FechaInicio { get; set; }
        public virtual DateTime FechaFin { get; set; }
        public virtual DateTime FechaAlta { get; set; }
        public virtual DateTime FechaModificacion { get; set; }
        public virtual bool Estatus { get; set; }
        public virtual Cedis Cedis { get; set; }
        public virtual IList<DetallePredefinida> DetallesPredefinida { get; set; }
    }
}
