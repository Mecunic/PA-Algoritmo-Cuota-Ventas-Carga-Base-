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
        public virtual string IdProducto { get; set; }
        public virtual string SKU { get; set; }
        public virtual string Descripcion { get; set; }
        public virtual TipoEmpaque TipoEmpaque { get; set; }
        public virtual UnidadEmpaque UnidadEmpaque { get; set; }
        public virtual Categoria Categoria { get; set; }
        public virtual Presentacion Presentacion { get; set; }
        public virtual DateTime FechaAlta { get; set; }
        public virtual DateTime FechaModificacion { get; set; }
        public virtual bool Estatus { get; set; }
    }
}
