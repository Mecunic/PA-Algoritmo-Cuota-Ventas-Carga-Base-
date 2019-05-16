﻿using System;
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

        public PaymentMethodModel PaymentMethod { get; set; }
}

    public class PaymentMethodModel
    {
        public string Type { get; set; }
        public string BankName { get; set; }

        public string Clabe { get; set; }

        public string Reference { get; set; }

        public string Name { get; set; }
        public string Agreement { get; set; }
    }

        public static class PaymentMethod
    {
         public const string
            CARD = "card",
            BANK_ACCOUNT = "bank_account";
    }

    public static class PaymentStatus
    {
        public const string
           IN_PROGRESS = "in_progress",
           COMPLETED = "completed",
           ERROR = "Error";
    }

    public static class PaymentEventStatus
    {
        public const string
           CHARGE_SUCCEEDED = "charge.succeeded";
    }

    public static class PaymentType
    {
        public const string
           CHARGE = "charge";
    }
}