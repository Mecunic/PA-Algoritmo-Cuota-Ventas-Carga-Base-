using MVC_Project.Domain.Entities;
using MVC_Project.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Project.Domain.Services
{
    #region Interfaces  

    public interface IOrderService : IService<Order>
    {
    }
    #endregion
    public class OrderService : ServiceBase<Order>, IOrderService
    {
        private IRepository<Order> _repository;
        public OrderService(IRepository<Order> baseRepository) : base(baseRepository)
        {
            _repository = baseRepository;
        }
    }
}
