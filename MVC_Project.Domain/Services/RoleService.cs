using MVC_Project.Domain.Entities;
using MVC_Project.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVC_Project.Domain.Services {
    #region Interfaces  
    public interface IRoleService : IService<Role>
    {
    }
    #endregion

    public class RoleService : ServiceBase<Role>, IRoleService
    {
        public RoleService(IRepository<Role> baseRepository) : base(baseRepository)
        {
        }
    }
}