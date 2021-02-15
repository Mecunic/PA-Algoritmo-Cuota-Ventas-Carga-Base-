using MVC_Project.Domain.Entities;
using MVC_Project.Domain.Repositories;
using MVC_Project.Domain.Services;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Criterion;

namespace MVC_Project.Data.Services
{
    public class ProductoService : ServiceBase<Producto>, IProductoService
    {
        private IRepository<Producto> _repository;
        public ProductoService(IRepository<Producto> baseRepository) : base(baseRepository)
        {
            _repository = baseRepository;
        }

        public override Tuple<IEnumerable<Producto>, int> FilterBy(NameValueCollection filtersValue, int? skip, int? take)
        {
            string FilterName = filtersValue.Get("Filtro").Trim();

            var query = _repository.Session.QueryOver<Producto>();
            if (!string.IsNullOrWhiteSpace(FilterName))
            {
                query = query.Where(producto => producto.Descripcion.IsInsensitiveLike("%" + FilterName + "%") || producto.TipoEmpaque.Name.IsInsensitiveLike("%" + FilterName + "%") || producto.Presentacion.Name.IsInsensitiveLike("%" + FilterName + "%"));
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
            var list = query.OrderBy(u => u.CreatedAt).Desc.List();
            return new Tuple<IEnumerable<Producto>, int>(list, count);
        }
    }
}
