using MVC_Project.Domain.Entities;
using MVC_Project.Domain.Repositories;
using MVC_Project.Domain.Services;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using NHibernate.Criterion;

namespace MVC_Project.Data.Services
{
    public class DetallePredefinidaService : ServiceBase<DetallePredefinida>, IDetallefinidaService
    {
        private IRepository<DetallePredefinida> _repository;
        public DetallePredefinidaService(IRepository<DetallePredefinida> baseRepository) : base(baseRepository)
        {
            _repository = baseRepository;
        }

        public void CreateAll(IList<DetallePredefinida> items)
        {
            _repository.Session.BeginTransaction();

            foreach (var item in items)
            {
                _repository.Session.Save(item);
            }

            _repository.Session.Transaction.Commit();
        }

        public Tuple<IEnumerable<DetallePredefinida>, int> FilterBy(int masterListId, NameValueCollection filtersValue, int? skip, int? take)
        {
            string FilterName = filtersValue.Get("Filtro")?.Trim();

            var query = _repository.Session.QueryOver<DetallePredefinida>();

            query = query.Where(x => x.ListaPredefinida.Id == masterListId);

            if (!string.IsNullOrWhiteSpace(FilterName))
            {
                query = query.Where(x => x.Producto.Descripcion.IsInsensitiveLike("%" + FilterName + "%"));
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
            return new Tuple<IEnumerable<DetallePredefinida>, int>(list, count);
        }
    }
}
