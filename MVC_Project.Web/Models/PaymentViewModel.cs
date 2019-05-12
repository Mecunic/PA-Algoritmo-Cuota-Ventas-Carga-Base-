using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_Project.Web.Models
{
    public class PaymentViewModel
    {
        public string Id { get; set; }
        public string OrderId { get; set; }
        public decimal Amount { get; set; }

        public DateTime? DueDate { get; set; }

        public string PaymentCardURL { get; set; }

        public string TokenId { get; set; }

        public string DeviceSessionId { get; set; }

        public string JsonData { get; set; }
    }
}