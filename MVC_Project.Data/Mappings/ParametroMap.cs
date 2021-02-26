using FluentNHibernate.Mapping;
using MVC_Project.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Project.Data.Mappings
{
    public class ParametroMap : ClassMap<Parametro>
    {
        public ParametroMap()
        {
            Table("dbo.Parametros");
            Id(x => x.Id).GeneratedBy.Identity().Column("IdParametro");
            Map(x => x.Clave).Column("Clave").Not.Nullable();
            Map(x => x.Nombre).Column("Nombre").Not.Nullable();
            Map(x => x.Valor).Column("Valor").Not.Nullable();
            Map(x => x.Tipo).Column("Tipo").Not.Nullable();
            Map(x => x.AlgoritmoUso).Column("AlgoritmoUso").Not.Nullable();
            Map(x => x.FechaAlta).Column("FechaAlta").Not.Nullable();
            Map(x => x.FechaModificacion).Column("FechaModificacion").Not.Nullable();
            Map(x => x.FechaBaja).Column("FechaBaja").Nullable();
            Map(x => x.Estatus).Column("Estatus").Not.Nullable();
            Map(x => x.Uuid).Column("Uuid").Not.Nullable();
        }
    }
}
