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
    public class CedisService : ServiceBase<Cedis>, ICedisService
    {
        private IRepository<Cedis> _repository;
        public CedisService(IRepository<Cedis> baseRepository) : base(baseRepository)
        {
            _repository = baseRepository;
        }
    }
}
