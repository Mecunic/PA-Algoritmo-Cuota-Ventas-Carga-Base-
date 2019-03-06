using FluentNHibernate.Mapping;
using MVC_Project.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Project.Data.Mappings
{
    public class RolePermissionMap : ClassMap<RolePermission>
    {
        public RolePermissionMap()
        {
            Table("permission_role");
            Id(x => x.Id).GeneratedBy.Identity().Column("id");
            References(x => x.Role).Column("role_id");
            References(x => x.Permission).Column("permission_id");
        }
    }
}
