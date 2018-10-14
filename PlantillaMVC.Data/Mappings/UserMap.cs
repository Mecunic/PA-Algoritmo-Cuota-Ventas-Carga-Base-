using FluentNHibernate.Automapping;
using FluentNHibernate.Mapping;
using PlantillaMVC.Domain.Entities;

namespace PlantillaMVC.Data.Mappings {

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
        }
    }
}