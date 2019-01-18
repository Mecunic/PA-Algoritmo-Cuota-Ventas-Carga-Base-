using MVC_Project.Domain.Entities;
using MVC_Project.Domain.Repositories;
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
        public UserService(IRepository<User> baseRepository) : base(baseRepository)
        {
        }
    }
}