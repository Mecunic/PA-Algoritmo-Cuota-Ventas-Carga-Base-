using MVC_Project.Domain.Entities;
using MVC_Project.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVC_Project.Domain.Services
{
    public interface IRoleService : IService<Role>
    {
        IList<Role> ObtenerRoles(string filtros);
    }
}