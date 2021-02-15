using FluentNHibernate.Automapping;
using FluentNHibernate.Mapping;
using MVC_Project.Domain.Entities;

namespace MVC_Project.Data.Mappings {

    public class UserMap : ClassMap<User> {

        public UserMap() {
            Table("dbo.users");
            Id(x => x.Id).GeneratedBy.Identity().Column("IdUser");
            Map(x => x.Uuid).Column("uuid").Not.Nullable();
            Map(x => x.FirstName).Column("first_name").Not.Nullable();
            Map(x => x.ApellidoPaterno).Column("last_name").Nullable();
            Map(x => x.Email).Column("email").Not.Nullable();
            Map(x => x.Password).Column("password").Not.Nullable();            
            Map(x => x.CreatedAt).Column("created_at").Not.Nullable();
            Map(x => x.UpdatedAt).Column("updated_at").Not.Nullable();
            Map(x => x.RemovedAt).Column("removed_at").Nullable();
            Map(x => x.Status).Column("status").Nullable();
            Map(x => x.ExpiraToken).Column("expira_token").Nullable();
            Map(x => x.Token).Column("token").Nullable();
            Map(x=>x.LastLoginAt).Column("last_login_at").Nullable();
            Map(x => x.Username).Column("username").Nullable();
            Map(x => x.PasswordExpiration).Column("password_expiration").Nullable();
            Map(x => x.ApellidoMaterno).Column("apellido_materno").Nullable();
            References(x => x.Role).Column("IdRole");
            References(x => x.Cedis).Column("IdCedis");
            HasManyToMany(x => x.Permissions)
                .Cascade.SaveUpdate()
                .Table("PermissionsUser")
                .ParentKeyColumn("IdUser")
                .ChildKeyColumn("IdPermission");
        }
    }
}