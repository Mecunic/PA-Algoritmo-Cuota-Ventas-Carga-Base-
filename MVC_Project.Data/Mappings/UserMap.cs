using FluentNHibernate.Automapping;
using FluentNHibernate.Mapping;
using MVC_Project.Domain.Entities;

namespace MVC_Project.Data.Mappings
{

    public class UserMap : ClassMap<User>
    {

        public UserMap()
        {
            Table("cat.Usuarios");
            Id(x => x.Id).GeneratedBy.Identity().Column("IdUsuario");
            Map(x => x.Uuid).Column("Uuid").Not.Nullable();
            Map(x => x.Nombre).Column("Nombres").Not.Nullable();
            Map(x => x.Usuario).Column("Usuario").Not.Nullable();
            Map(x => x.IdCedis).Column("IdCedis").Not.Nullable();
            Map(x => x.Status).Column("Estatus").Not.Nullable();
            Map(x => x.CreatedAt).Column("FechaAlta").Not.Nullable();
            Map(x => x.UpdatedAt).Column("FechaModificacion").Not.Nullable();
            Map(x => x.RemovedAt).Column("FechaBaja").Nullable();

            References(x => x.Role).Column("IdRole");

            HasManyToMany(x => x.Permissions)
                .Cascade.SaveUpdate()
                .Table("PermisosUsuarios")
                .ParentKeyColumn("IdUsuario")
                .ChildKeyColumn("IdPermiso");
        }
    }
}