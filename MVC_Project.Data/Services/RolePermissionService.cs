using MVC_Project.Domain.Entities;
using MVC_Project.Domain.Repositories;
using MVC_Project.Domain.Services;

namespace MVC_Project.Data.Services
{
    public class RolePermissionService : ServiceBase<RolePermission>, IRolePermissionService
    {
        private IRepository<RolePermission> _repository;
        public RolePermissionService(IRepository<RolePermission> baseRepository) : base(baseRepository)
        {
            _repository = baseRepository;
        }
    }
}
