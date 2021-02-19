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
            Table("dbo.Cedis");
            Id(x => x.Id).GeneratedBy.Identity().Column("IdCedis");
            Map(x => x.Code).Column("code").Not.Nullable();
            Map(x => x.Name).Column("name").Not.Nullable();
            Map(x => x.Uuid).Column("uuid").Not.Nullable();
            Map(x => x.Description).Column("description").Not.Nullable();
            Map(x => x.CreatedAt).Column("created_at").Not.Nullable();
            Map(x => x.UpdatedAt).Column("updated_at").Not.Nullable();
            Map(x => x.RemovedAt).Column("removed_at").Nullable();
            Map(x => x.Status).Column("status").Nullable();
            HasMany(x => x.Users).Inverse().Cascade.All().KeyColumn("IdCedis");
            References(x => x.Manager).Column("manager");
        }
    }
}
