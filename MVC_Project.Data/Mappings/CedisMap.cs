using FluentNHibernate.Mapping;
using MVC_Project.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Project.Data.Mappings
{
    public class CedisMap : ClassMap<Cedis>
    {
        public CedisMap()
        {
            Table("cat.Cedis");
            Id(x => x.Id).GeneratedBy.Identity().Column("IdCedis");
            Map(x => x.Clave).Column("Clave").Not.Nullable();
            Map(x => x.Nombre).Column("Nombre").Not.Nullable();
            Map(x => x.Descripcion).Column("Descripcion").Not.Nullable();
            Map(x => x.FechaAlta).Column("FechaAlta").Not.Nullable();
            Map(x => x.FechaModificacion).Column("FechaModificacion").Not.Nullable();
            References(x => x.Responsable).Column("IdResponsable");
            HasMany(x => x.Usuarios).Inverse().Cascade.All().KeyColumn("IdCedis").LazyLoad();
        }
    }
}
