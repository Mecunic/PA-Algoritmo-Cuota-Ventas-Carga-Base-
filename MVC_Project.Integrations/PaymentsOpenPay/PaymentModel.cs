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

        public string ClientId { get; set; }

        public string OrderId { get; set; }
        public decimal Amount { get; set; }

        public DateTime? DueDate { get; set; }

        public string TransactionType { get; set; }

        public string Status { get; set; }

        public string PaymentCardURL { get; set; }

        public string TokenId { get; set; }

        public string DeviceSessionId { get; set; }
        
        public string ResultData { get; set; }

        public string ResultCategory{ get; set; }

        public bool ChargeSuccess { get; set; }

        public string Description { get; set; }
    }

    public static class PaymentMethod
    {
         public const string
            Card = "card",
            Bank_Account = "bank_account";
    }

    public static class PaymentStatus
    {
        public const string
           In_Progress = "in_progress",
           Completed = "completed",
           Error = "Error";
    }
}
