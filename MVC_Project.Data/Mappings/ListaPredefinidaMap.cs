using FluentNHibernate.Mapping;
using MVC_Project.Domain.Entities;

namespace MVC_Project.Data.Mappings
{
    public class ListaPredefinidaMap : ClassMap<ListaPredefinida>
    {
        public ListaPredefinidaMap()
        {
            Table("prd.ListasPredefinidas");
            Id(x => x.Id).GeneratedBy.Identity().Column("IdListaPredefinida");
            Map(x => x.FechaInicio).Column("FechaInicio").Not.Nullable();
            Map(x => x.FechaFin).Column("FechaFin").Not.Nullable();
            Map(x => x.FechaAlta).Column("FechaAlta").Not.Nullable();
            Map(x => x.FechaModificacion).Column("FechaModificacion").Not.Nullable();
            Map(x => x.Estatus).Column("Estatus").Not.Nullable();
            References(x => x.Cedis).Column("IdCedis");
            HasMany(x => x.DetallesPredefinida).Inverse().Cascade.All().KeyColumn("IdListaPredefinida").LazyLoad();
        }
    }
}
