using MVC_Project.Domain.Entities;
using MVC_Project.Domain.Repositories;
using MVC_Project.Domain.Services;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using NHibernate.Criterion;

namespace MVC_Project.Data.Services
{
    public class ListaPredefinidaService : ServiceBase<ListaPredefinida>, IListaPredefinidaService
    {
        private IRepository<ListaPredefinida> _repository;
        public ListaPredefinidaService(IRepository<ListaPredefinida> baseRepository) : base(baseRepository)
        {
            _repository = baseRepository;
        }

        public override Tuple<IEnumerable<ListaPredefinida>, int> FilterBy(NameValueCollection filtersValue, int? skip, int? take)
        {
            string FilterName = filtersValue.Get("Filtro")?.Trim();
            string statusFilter = filtersValue.Get("StatusFilter")?.Trim();

            var query = _repository.Session.QueryOver<ListaPredefinida>();
            if (!string.IsNullOrWhiteSpace(FilterName))
            {
                query = query.Where(x => x.Cedis.Nombre.IsInsensitiveLike("%" + FilterName + "%"));
            }
            if(!string.IsNullOrEmpty(statusFilter) && statusFilter.Equals("on", StringComparison.InvariantCultureIgnoreCase))
            {
                query = query.Where(x => x.Estatus == true || x.Estatus == false);
            } else
            {
                query = query.Where(x => x.Estatus == true);
            }
            var count = query.RowCount();

            if (skip.HasValue)
            {
                query.Skip(skip.Value);
            }

            if (take.HasValue)
            {
                query.Take(take.Value);
            }
            var list = query.OrderBy(u => u.FechaAlta).Desc.List();
            return new Tuple<IEnumerable<ListaPredefinida>, int>(list, count);
        }
    }
}
