using MVC_Project.Domain.Entities;
using MVC_Project.Domain.Helpers;
using MVC_Project.Domain.Repositories;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Project.Domain.Services
{
    #region Interfaces

    public interface IOrderService : IService<Order>
    {
    }

    #endregion Interfaces

    public class OrderService : ServiceBase<Order>, IOrderService
    {
        private IRepository<Order> _repository;

        public OrderService(IRepository<Order> baseRepository) : base(baseRepository)
        {
            _repository = baseRepository;
        }

        public IList<Order> FilterBy(string filtros)
        {
            filtros = filtros.Replace("[", "").Replace("]", "").Replace("\\", "").Replace("\"", "");
            var filters = filtros.Split(',').ToList();

            Customer customerAlias = null;
            Store storeAlias = null;
            Staff staffAlias = null;
            var query = _repository.Session.QueryOver<Order>()
            .JoinAlias(x => x.Store, () => storeAlias)
            .JoinAlias(x => x.Staff, () => staffAlias)
            .JoinAlias(x => x.Customer, () => customerAlias);
            if (!String.IsNullOrWhiteSpace(filters[0]))
            {
                query = query.WhereRestrictionOn(() => customerAlias.FirstName).IsInsensitiveLike("%" + filters[0] + "%");
            }
            if (!String.IsNullOrWhiteSpace(filters[2]))
            {
                DateTime Inicio = DateTime.Parse(filters[2]);
                query = query.Where(c => c.CreatedAt.Date >= Inicio);
            }
            if (!String.IsNullOrWhiteSpace(filters[3]))
            {
                DateTime Fin = DateTime.Parse(filters[3]);
                query = query.Where(c => c.CreatedAt.Date <= Fin);
            }

            return query.OrderBy(u => u.CreatedAt).Desc.List();
        }
    }
}