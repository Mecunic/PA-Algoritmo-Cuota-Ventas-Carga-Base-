using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Project.Domain.Entities
{
    public class Producto : IEntity
    {
        public virtual int Id { get; set; }
        public virtual  string SKU { get; set; }
        public virtual  string Descripcion { get; set; }
        public virtual TipoEmpaque TipoEmpaque { get; set; }
        public virtual UnidadEmpaque UnidadEmpaque { get; set; }
        public virtual Boolean ProductoEstrategico { get; set; }
        public virtual Categoria Categoria { get; set; }
        public virtual Presentacion Presentacion { get; set; }
        public virtual decimal PrecioUnitario { get; set; }
        public virtual decimal PrecioEmpaque { get; set; }
        public virtual DateTime? FechaInicio { get; set; }
        public virtual DateTime? FechaFin { get; set; }
        public virtual DateTime CreatedAt { get; set; }
        public virtual DateTime UpdatedAt { get; set; }
        public virtual DateTime? RemovedAt { get; set; }
        public virtual string Uuid { get; set; }
        public virtual Boolean Status { get; set; }
    }
}
