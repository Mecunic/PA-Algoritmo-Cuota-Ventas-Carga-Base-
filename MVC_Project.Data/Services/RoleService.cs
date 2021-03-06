using MVC_Project.Domain.Entities;
using MVC_Project.Domain.Repositories;
using MVC_Project.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVC_Project.Data.Services
{   
    public class RoleService : ServiceBase<Role>, IRoleService
    {
        private IRepository<Role> _repository;
        public RoleService(IRepository<Role> baseRepository) : base(baseRepository)
        {
            _repository = baseRepository;
        }
        public IList<Role> ObtenerRoles(string filtros)
        {
            filtros = filtros.Replace("[", "").Replace("]", "").Replace("\\", "").Replace("\"", "");
            var filters = filtros.Split(',').ToList();

            var roles = _repository.GetAll();
            if (!string.IsNullOrWhiteSpace(filters[0]))
            {
                string nombre = filters[0];
                roles = roles.Where(p => p.Name.ToLower().Contains(nombre.ToLower()));
            }
            //if (filters[1] != "2")
            //{
            //    bool status = filters[1] == "1" ? true : false;
            //    users = users.Where(p => p.Status == status);
            //}
            return roles.ToList();
        }
    }
}