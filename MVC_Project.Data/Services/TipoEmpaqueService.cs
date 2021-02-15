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
    public class TipoEmpaqueService : ServiceBase<TipoEmpaque>, ITipoEmpaqueService
    {
        private IRepository<TipoEmpaque> _repository;
        public TipoEmpaqueService(IRepository<TipoEmpaque> baseRepository) : base(baseRepository)
        {
            _repository = baseRepository;
        }
    }
}
