using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Project.Integrations.Payments
{
    public interface IPaymentServiceProvider
    {
        PaymentModel CreateBankTransferPayment(PaymentModel payment);
        PaymentModel CreateTDCPayment(PaymentModel payment);
    }
}
