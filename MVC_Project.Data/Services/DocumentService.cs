using MVC_Project.Domain.Entities;
using MVC_Project.Domain.Repositories;
using MVC_Project.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using NHibernate.Criterion;
using MVC_Project.Domain.Services;

namespace MVC_Project.Data.Services
{
    public class DocumentService : ServiceBase<Document>, IDocumentService
    {
        private IRepository<Document> _repository;

        public DocumentService(IRepository<Document> baseRepository) : base(baseRepository)
        {
            _repository = baseRepository;
        }

        public Tuple<IEnumerable<Document>, int> FilterBy(NameValueCollection filtersValue, int? skip, int? take)
        {
            var query = _repository.Session.QueryOver<Document>();
            
            string FilterName = filtersValue.Get("Name").Trim();
            string FilterDateInitial = filtersValue.Get("FilterDateInitial").Trim();
            string FilterDateEnd = filtersValue.Get("FilterDateEnd").Trim();
            int FilterUser = Convert.ToInt32(filtersValue.Get("UserId").Trim());
            DateTime? initialDate = DateUtil.ToDateTime(FilterDateInitial, Constants.DATE_FORMAT);
            DateTime? endDate = DateUtil.ToDateTime(FilterDateEnd, Constants.DATE_FORMAT);

            if (FilterUser > 0)
            {
                query = query.Where(x => x.User.Id == FilterUser);
            }
            if (!string.IsNullOrWhiteSpace(FilterName))
            {
                query = query.Where(x => x.Name.IsInsensitiveLike("%" + FilterName + "%"));
            }
            if (initialDate.HasValue)
            {
                query = query.Where(x => x.CreationDate >= initialDate.Value);
            }
            if (endDate.HasValue)
            {
                query = query.Where(x => x.CreationDate <= endDate.Value);
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
            var list = query.OrderBy(u => u.CreationDate).Desc.List();

            //list = this.GetAll().ToList<Document>();

            return new Tuple<IEnumerable<Document>, int>(list, count);
        }

    }
}
