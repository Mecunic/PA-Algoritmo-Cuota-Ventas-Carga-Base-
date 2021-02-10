using MVC_Project.Domain.Entities;
using MVC_Project.Domain.Repositories;
using MVC_Project.Domain.Services;
using MVC_Project.Utils;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace MVC_Project.Data.Services
{
    public class UserService : ServiceBase<User>, IUserService
    {
        private IRepository<User> _repository;

        public UserService(IRepository<User> baseRepository) : base(baseRepository)
        {
            _repository = baseRepository;
        }

        public bool Exists(string email)
        {
            return _repository.FindBy(u => u.Email.Trim().ToLower().Equals(email.Trim().ToLower())).Any();
        }

        public Tuple<IEnumerable<User>, int>  FilterBy(NameValueCollection filtersValue, int? skip, int? take)
        {
            string FilterName = filtersValue.Get("Name").Trim();
            
            var query = _repository.Session.QueryOver<User>();
            if (!string.IsNullOrWhiteSpace(FilterName))
            {
                query = query.Where(user => user.Email.IsInsensitiveLike("%" + FilterName + "%") || user.FirstName.IsInsensitiveLike("%" + FilterName + "%") || user.ApellidoPaterno.IsInsensitiveLike("%" + FilterName + "%"));
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
            return new Tuple<IEnumerable<User>, int>(list, count);
        }
    }
}