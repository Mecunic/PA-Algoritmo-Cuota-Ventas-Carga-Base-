using System;

namespace MVC_Project.Domain.Entities
{
    public class DetallePredefinida : IEntity
    {
        public virtual int Id { get; set; }
        public virtual int Cantidad { get; set; }
        public virtual bool Estrategico { get; set; }
        public virtual bool Prioritario { get; set; }
        public virtual bool Tactico { get; set; }
        public virtual DateTime FechaAlta { get; set; }
        public virtual DateTime FechaModificacion { get; set; }
        public virtual bool Estatus { get; set; }

        public virtual ListaPredefinida ListaPredefinida { get; set; }
        public virtual Producto Producto { get; set; }
    }
}
