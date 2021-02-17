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
            Table("dbo.Productos");
            Id(x => x.Id).GeneratedBy.Identity().Column("IdProducto");
            Map(x => x.SKU).Column("SKU").Not.Nullable();
            Map(x => x.Descripcion).Column("Descripcion").Not.Nullable();
            Map(x => x.PrecioEmpaque).Column("PrecioEmpaque").Not.Nullable();
            Map(x => x.PrecioUnitario).Column("PrecioUnitario").Not.Nullable();
            Map(x => x.ProductoEstrategico).Column("ProductoEstrategico").Not.Nullable();
            Map(x => x.FechaInicio).Column("FechaInicio").Nullable();
            Map(x => x.FechaFin).Column("FechaFin").Nullable();
            Map(x => x.CreatedAt).Column("created_at").Not.Nullable();
            Map(x => x.UpdatedAt).Column("updated_at").Not.Nullable();
            Map(x => x.RemovedAt).Column("removed_at").Nullable();
            Map(x => x.Status).Column("status").Not.Nullable();
            Map(x => x.Uuid).Column("uuid").Not.Nullable();
            References(x => x.Categoria).Column("IdCategoria");
            References(x => x.Presentacion).Column("IdPresentacion");
            References(x => x.TipoEmpaque).Column("IdTipoEmpaque");
            References(x => x.UnidadEmpaque).Column("IdUnidadEmpaque");
        }
    }
}
