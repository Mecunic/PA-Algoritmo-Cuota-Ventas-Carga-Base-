using MVC_Project.Domain.Entities;
using MVC_Project.Domain.Repositories;
using MVC_Project.Domain.Services;

namespace MVC_Project.Data.Services
{   
    public class PermissionService : ServiceBase<Permission>, IPermissionService
    {
        public PermissionService(IRepository<Permission> baseRepository) : base(baseRepository)
        {
        }
    }
}