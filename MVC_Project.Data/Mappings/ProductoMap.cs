using FluentNHibernate.Mapping;
using MVC_Project.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Project.Data.Mappings
{
    public class ProductoMap : ClassMap<Producto>
    {
        public ProductoMap()
        {
            Table("cat.Productos");
            //Id(x => x.Id).GeneratedBy.Identity().Column("IdProducto");
            Id(x => x.IdProducto).Column("IdProducto");
            Map(x => x.SKU).Column("SKU").Not.Nullable();
            Map(x => x.Descripcion).Column("Descripcion").Not.Nullable();
            Map(x => x.FechaAlta).Column("FechaAlta").Not.Nullable();
            Map(x => x.FechaModificacion).Column("FechaModificacion").Not.Nullable();
            Map(x => x.Estatus).Column("Estatus").Not.Nullable();
            References(x => x.Categoria).Column("IdCategoria");
            References(x => x.Presentacion).Column("IdPresentacion");
            References(x => x.TipoEmpaque).Column("IdTipoEmpaque");
            References(x => x.UnidadEmpaque).Column("IdUnidadEmpaque");
        }
    }
}
