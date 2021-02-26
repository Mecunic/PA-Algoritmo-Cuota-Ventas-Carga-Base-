using FluentNHibernate.Mapping;
using MVC_Project.Domain.Entities;

namespace MVC_Project.Data.Mappings
{
    public class DetallePredefinidaMap : ClassMap<DetallePredefinida>
    {
        public DetallePredefinidaMap()
        {
            Table("prd.DetallesPredefinidas");
            Id(x => x.Id).GeneratedBy.Identity().Column("IdDetallePredefinida");
            Map(x => x.Cantidad).Column("Cantidad").Not.Nullable();
            Map(x => x.Estrategico).Column("Estrategico").Not.Nullable();
            Map(x => x.Prioritario).Column("Prioritario").Not.Nullable();
            Map(x => x.Tactico).Column("Tactico").Not.Nullable();
            Map(x => x.FechaAlta).Column("FechaAlta").Not.Nullable();
            Map(x => x.FechaModificacion).Column("FechaModificacion").Not.Nullable();
            Map(x => x.Estatus).Column("Estatus").Not.Nullable();
            References(x => x.ListaPredefinida).Column("IdListaPredefinida");
            References(x => x.Producto).Column("IdProducto");
        }
    }
}
