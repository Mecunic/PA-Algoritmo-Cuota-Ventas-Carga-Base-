using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Project.Integrations.OpenPay
{
    public class PaymentEventModel
    {
        public string id { get; set; }
        public string type { get; set; }

        public string event_date { get; set; }

        public DateTime? creation_date { get; set; }

        public PaymentTransactionModel transaction { get; set; }

        public string verification_code { get; set; }
    }


    public class PaymentTransactionModel
    {
        public string id { get; set; }
        public string authorization { get; set; }

        public string operation_type { get; set; }

        public string method { get; set; }

        public string transaction_type { get; set; }

        public string status { get; set; }

        public bool conciliated { get; set; }
    }
}
