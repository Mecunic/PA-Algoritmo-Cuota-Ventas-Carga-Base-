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
        public IList<User> ObtenerUsuarios(string filtros)
        {
            filtros = filtros.Replace("[", "").Replace("]", "").Replace("\\", "").Replace("\"", "");
            var filters = filtros.Split(',').ToList();

            var users = _repository.GetAll();
            if (!string.IsNullOrWhiteSpace(filters[0]))
            {
                string nombre = filters[0];
                users = users.Where(p => p.FirstName.ToLower().Contains(nombre.ToLower()));
            }
            if (filters[1] != "2")
            {
                bool status = filters[1]=="1"?true:false;
                users = users.Where(p => p.Status == status);
            }
            return users.ToList();
        }

    }
}