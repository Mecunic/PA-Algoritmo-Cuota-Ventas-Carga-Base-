using MVC_Project.Domain.Entities;
using MVC_Project.Domain.Repositories;
using MVC_Project.Domain.Services;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using NHibernate.Criterion;

namespace MVC_Project.Data.Services
{
    public class CedisService : ServiceBase<Cedis>, ICedisService
    {
        private IRepository<Cedis> _repository;
        public CedisService(IRepository<Cedis> baseRepository) : base(baseRepository)
        {
            _repository = baseRepository;
        }

        public override Tuple<IEnumerable<Cedis>, int> FilterBy(NameValueCollection filtersValue, int? skip, int? take)
        {
            string FilterCode = filtersValue.Get("Code").Trim();
            string FilterName = filtersValue.Get("Name").Trim();

            var query = _repository.Session.QueryOver<Cedis>();
            if (!string.IsNullOrWhiteSpace(FilterCode))
            {
                query = query.Where(cedi => cedi.Clave.IsInsensitiveLike("%" + FilterCode + "%"));
            }
            if (!string.IsNullOrWhiteSpace(FilterName))
            {
                query = query.Where(cedi => cedi.Nombre.IsInsensitiveLike("%" + FilterName + "%"));
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
            return new Tuple<IEnumerable<Cedis>, int>(list, count);
        }
    }
}
