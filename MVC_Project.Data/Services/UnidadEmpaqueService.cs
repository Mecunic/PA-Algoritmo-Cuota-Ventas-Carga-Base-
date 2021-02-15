using MVC_Project.Domain.Entities;
using MVC_Project.Domain.Repositories;
using MVC_Project.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Project.Data.Services
{
    public class UnidadEmpaqueService : ServiceBase<UnidadEmpaque>, IUnidadEmpaqueService
    {
        private IRepository<UnidadEmpaque> _repository;
        public UnidadEmpaqueService(IRepository<UnidadEmpaque> baseRepository) : base(baseRepository)
        {
            _repository = baseRepository;
        }
    }
}
