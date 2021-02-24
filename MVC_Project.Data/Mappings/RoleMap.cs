using FluentNHibernate.Automapping;
using FluentNHibernate.Mapping;
using MVC_Project.Domain.Entities;

namespace MVC_Project.Data.Mappings
{

    public class RoleMap : ClassMap<Role>
    {
        public RoleMap()
        {
            Table("dbo.Roles");
            Id(x => x.Id).GeneratedBy.Identity().Column("IdRole");
            Map(x => x.Code).Column("Clave").Not.Nullable();
            Map(x => x.Name).Column("Nombre").Not.Nullable();
            Map(x => x.Uuid).Column("Uuid").Not.Nullable();
            Map(x => x.Description).Column("Descripcion").Not.Nullable();
            Map(x => x.CreatedAt).Column("FechaAlta").Not.Nullable();
            Map(x => x.UpdatedAt).Column("FechaModificacion").Not.Nullable();
            Map(x => x.RemovedAt).Column("FechaBaja").Nullable();
            Map(x => x.Status).Column("Estatus").Not.Nullable();
            HasMany(x => x.Users).Inverse().Cascade.All().KeyColumn("IdRole");
            HasManyToMany(x => x.Permissions).Cascade.All().Table("PermisosRoles").ParentKeyColumn("IdRole").ChildKeyColumn("IdPermiso");
        }
    }
}