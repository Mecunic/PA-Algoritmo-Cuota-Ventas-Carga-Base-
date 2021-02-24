using FluentNHibernate.Automapping;
using FluentNHibernate.Mapping;
using MVC_Project.Domain.Entities;

namespace MVC_Project.Data.Mappings
{

    public class PermissionMap : ClassMap<Permission>
    {
        public PermissionMap()
        {
            Table("dbo.Permisos");
            Id(x => x.Id).GeneratedBy.Identity().Column("IdPermiso");
            Map(x => x.Description).Column("Descripcion").Not.Nullable();
            Map(x => x.Controller).Column("Controlador").Not.Nullable();
            Map(x => x.Action).Column("Accion").Not.Nullable();
            Map(x => x.Module).Column("Modulo").Nullable();
            Map(x => x.CreatedAt).Column("FechaAlta").Not.Nullable();
            Map(x => x.UpdatedAt).Column("FechaModificacion").Not.Nullable();
            Map(x => x.RemovedAt).Column("FechaBaja").Nullable();
        }
    }
}