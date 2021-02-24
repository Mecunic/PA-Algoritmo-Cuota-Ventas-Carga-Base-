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
            Map(x => x.FirstName).Column("Nombres").Not.Nullable();
            Map(x => x.ApellidoPaterno).Column("ApellidoPaterno").Not.Nullable();
            Map(x => x.ApellidoMaterno).Column("ApellidoMaterno").Nullable();
            Map(x => x.Email).Column("Correo").Not.Nullable();
            Map(x => x.Password).Column("Password").Not.Nullable();
            Map(x => x.PasswordExpiration).Column("ExpiracionPassword").Nullable();
            Map(x => x.Status).Column("Estatus").Not.Nullable();
            Map(x => x.Token).Column("Token").Nullable();
            Map(x => x.ExpiraToken).Column("Expiraciontoken").Nullable();
            Map(x => x.CreatedAt).Column("FechaAlta").Not.Nullable();
            Map(x => x.UpdatedAt).Column("FechaModificacion").Not.Nullable();
            Map(x => x.RemovedAt).Column("FechaBaja").Nullable();
            
            References(x => x.Role).Column("IdRole");
            References(x => x.Cedis).Column("IdCedis");
            HasManyToMany(x => x.Permissions)
                .Cascade.SaveUpdate()
                .Table("PermisosUsuarios")
                .ParentKeyColumn("IdUsuario")
                .ChildKeyColumn("IdPermiso");
        }
    }
}