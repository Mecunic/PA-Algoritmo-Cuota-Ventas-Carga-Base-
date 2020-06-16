using FluentNHibernate.Mapping;
using MVC_Project.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Project.Data.Mappings
{
    public class DocumentMap : ClassMap<Document>
    {
        public DocumentMap()
        {
            Table("dbo.documents");
            Id(x => x.Id).GeneratedBy.Identity().Column("DocumentId");
            References(x => x.User).Column("UserId").Not.Nullable().LazyLoad().Fetch.Join();
            Map(x => x.Uuid).Column("uuid").Not.Nullable();
            Map(x => x.Type).Column("type").Nullable();
            Map(x => x.Name).Column("name").Nullable();
            Map(x => x.CreationDate).Column("creation_date").Not.Nullable();
            Map(x => x.URL).Column("url").Nullable();
            //Map(x => x.Blocked).Column("blocked").Default("0").Nullable();
        }
    }
}
