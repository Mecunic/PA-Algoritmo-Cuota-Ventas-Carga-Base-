using FluentNHibernate.Automapping;
using FluentNHibernate.Mapping;
using MVC_Project.Domain.Entities;

namespace MVC_Project.Data.Mappings {

    public class UserMap : ClassMap<User> {

        public UserMap() {
            Table("users");
            Id(x => x.Id).GeneratedBy.Identity().Column("id");
            Map(x => x.Name).Column("name").Not.Nullable();
            Map(x => x.Email).Column("email").Not.Nullable();
            Map(x => x.Password).Column("password").Not.Nullable();            
            Map(x => x.CreatedAt).Column("created_at").Not.Nullable();
            Map(x => x.UpdatedAt).Column("updated_at").Not.Nullable();
            Map(x => x.RemovedAt).Column("removed_at").Nullable();
            References(x => x.Role).Column("role_id");
            HasManyToMany(x => x.Permissions).Cascade.SaveUpdate().Table("permission_user").ParentKeyColumn("user_id").ChildKeyColumn("permission_id");
        }
    }
}