using MVC_Project.Domain.Entities;
using MVC_Project.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Criterion;
using MVC_Project.Domain.Services;

namespace MVC_Project.Data.Services
{
    public class ParametroService : ServiceBase<Parametro>, IParametroService
    {
        private IRepository<Parametro> _repository;
        public ParametroService(IRepository<Parametro> baseRepository) : base(baseRepository)
        {
            _repository = baseRepository;
        }

        public override Tuple<IEnumerable<Parametro>, int> FilterBy(NameValueCollection filtersValue, int? skip, int? take)
        {
            string FilterName = filtersValue.Get("Filtro").Trim();

            var query = _repository.Session.QueryOver<Parametro>();
            if (!string.IsNullOrWhiteSpace(FilterName))
            {
                query = query.Where(parametro => parametro.Clave.IsInsensitiveLike("%" + FilterName + "%") || parametro.Nombre.IsInsensitiveLike("%" + FilterName + "%") || parametro.Tipo.IsInsensitiveLike("%" + FilterName + "%") || parametro.Valor.IsInsensitiveLike("%" + FilterName + "%") || parametro.AlgoritmoUso.IsInsensitiveLike("%" + FilterName + "%"));
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
            return new Tuple<IEnumerable<Parametro>, int>(list, count);
        }
    }
}
