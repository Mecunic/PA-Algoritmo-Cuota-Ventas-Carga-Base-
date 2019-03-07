using MVC_Project.Domain.Entities;
using MVC_Project.Domain.Repositories;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVC_Project.Domain.Services {
    #region Interfaces  
    
    public interface IUserService : IService<User>
    {
    }
    #endregion
    public class UserService : ServiceBase<User>, IUserService
    {
        private IRepository<User> _repository;
        
        public UserService(IRepository<User> baseRepository) : base(baseRepository)
        {
            _repository = baseRepository;
        }
        public IList<User> ObtenerUsuarios(string filtros, ISession session)
        {
            filtros = filtros.Replace("[", "").Replace("]", "").Replace("\\", "").Replace("\"", "");
            var filters = filtros.Split(',').ToList();

            var query = session.QueryOver<User>();
            if (!string.IsNullOrWhiteSpace(filters[0]))
            {
                string nombre = filters[0];
                query = query.Where(x => (x.Email.IsInsensitiveLike("%" + nombre + "%") || x.FirstName.IsInsensitiveLike("%" + nombre + "%")));

            }
            if (filters[1] != "2")
            {
                bool status = filters[1]=="1"?true:false;
                query = query.Where(p => p.Status == status);
            }
            return query.OrderBy(u => u.CreatedAt).Desc.List();
        }

    }
}