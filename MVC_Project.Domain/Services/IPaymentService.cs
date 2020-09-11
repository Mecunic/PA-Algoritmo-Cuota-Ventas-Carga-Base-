using MVC_Project.Domain.Entities;
using MVC_Project.Domain.Repositories;
using MVC_Project.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using NHibernate.Criterion;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Project.Domain.Services
{
    public interface IPaymentService : IService<Payment>
    {
        Payment GetByOrderId(string OrderId);
        Payment GetByProviderId(string ProviderId);
        PaymentApplication GetPaymentApplicationByKey(string appKey);
        Tuple<IEnumerable<Payment>, int> FilterBy(NameValueCollection filtersValue, int? skip, int? take);
    }
}
