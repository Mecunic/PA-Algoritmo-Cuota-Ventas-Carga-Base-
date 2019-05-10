using MVC_Project.Domain.Entities;
using MVC_Project.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Project.Domain.Services
{
    public interface IPaymentService : IService<Payment>
    {
    }

    public class PaymentService : ServiceBase<Payment>, IPaymentService
    {
        private IRepository<Payment> _repository;

        public PaymentService(IRepository<Payment> baseRepository) : base(baseRepository)
        {
            _repository = baseRepository;
        }

        public Payment GetByOrderTransaction(string OrderId, string ProviderId)
        {
            var payments = _repository.Session.QueryOver<Payment>().Where( x=> x.OrderId == OrderId && x.ProviderId == ProviderId);
            return payments.List().FirstOrDefault();
        }
    }
}
