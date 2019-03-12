using MVC_Project.Domain.Entities;
using MVC_Project.Domain.Repositories;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace MVC_Project.Domain.Services
{
    #region Interfaces

    public interface IUserService : IService<User>
    {
    }

    #endregion Interfaces

    public class UserService : ServiceBase<User>, IUserService
    {
        private IRepository<User> _repository;

        public UserService(IRepository<User> baseRepository) : base(baseRepository)
        {
            _repository = baseRepository;
        }

        public IEnumerable<User> FilterBy(string filtros)
        {
            filtros = filtros.Replace("[", "").Replace("]", "").Replace("\\", "").Replace("\"", "");
            var filters = filtros.Split(',').ToList();
            var filterSearchString = filters[0];
            var filterStatus = filters[1];
            var users = _repository.Session.QueryOver<User>();
            if (!string.IsNullOrWhiteSpace(filterSearchString))
            {
                users = users.Where(user => user.Email.IsInsensitiveLike("%" + filterSearchString + "%") || user.FirstName.IsInsensitiveLike("%" + filterSearchString + "%") || user.LastName.IsInsensitiveLike("%" + filterSearchString + "%"));
            }
            if (filterStatus != "2")
            {
                bool status = filterStatus == "1" ? true : false;
                users = users.Where(user => user.Status == status);
            }
            return users.OrderBy(u => u.CreatedAt).Desc.List();
        }
    }
}