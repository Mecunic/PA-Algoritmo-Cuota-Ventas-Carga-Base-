using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Project.Integrations.PaymentsOpenPay
{
    public class PaymentModel
    {
        public string Id { get; set; }

        public string OrderId { get; set; }
        public decimal Amount { get; set; }

        public DateTime? DueDate { get; set; }

        public string TransactionType { get; set; }

        public string Status { get; set; }

        public string PaymentCardURL { get; set; }

        public string JsonData { get; set; }
    }
}
