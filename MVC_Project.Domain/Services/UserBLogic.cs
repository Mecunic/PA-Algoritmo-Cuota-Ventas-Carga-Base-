using MVC_Project.Domain.Entities;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Project.Domain.Services
{
    public class UserBLogic
    {
        private ISession session;
        public UserBLogic(ISession session)
        {
            this.session = session;
        }

        public IList<User> ObtenerUsuarios(string filtros)
        {
            filtros = filtros.Replace("[", "").Replace("]", "").Replace("\\", "").Replace("\"", "");
            var filters = filtros.Split(',').ToList();
            var query = session.QueryOver<User>();
            if (!String.IsNullOrWhiteSpace(filters[0]))
            {
                query = query.WhereRestrictionOn(u => u.FirstName).IsInsensitiveLike("%" + filters[0] + "%");
            }
            if (filters[1] != "2")
            {
                bool status = filters[1] == "1" ? true : false;
                query = query.Where(u => u.Status == status);
            }

            return query.OrderBy(u => u.FirstName).Desc.List();

        }
    }
}
