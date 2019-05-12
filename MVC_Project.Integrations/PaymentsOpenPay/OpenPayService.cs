using Openpay;
using Openpay.Entities;
using Openpay.Entities.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Project.Integrations.PaymentsOpenPay
{
    public class OpenPayService
    {
        string OpenpayKey;
        string MerchantId;
        string DashboardURL;

        public OpenPayService()
        {
            OpenpayKey = System.Configuration.ConfigurationManager.AppSettings["Payments.OpenpayKey"];
            MerchantId = System.Configuration.ConfigurationManager.AppSettings["Payments.MerchantId"];
            DashboardURL = System.Configuration.ConfigurationManager.AppSettings["Payments.DashboardURL"];
        }

        public PaymentModel CreateSPEIPayment(PaymentModel payment)
        {
            OpenpayAPI openpayAPI = new OpenpayAPI(OpenpayKey, MerchantId);
            openpayAPI.Production = false;

            Customer customer = new Customer
            {
                Name = "Test ",
                LastName = "Client",
                Email = "test@ensitech.com"
            };

            Customer customerCreated = openpayAPI.CustomerService.Create(customer);

            ChargeRequest request = new ChargeRequest
            {
                OrderId = payment.OrderId,
                Amount = payment.Amount,
                DueDate = DateTime.Now.AddDays(2),
                Method = "bank_account",
                Description = "Pago SPEI Prueba",
            };

            Charge charge = openpayAPI.ChargeService.Create(customerCreated.Id, request);

            payment.Id = charge.Id;
            payment.DueDate = request.DueDate;
            payment.Status = charge.Status;
            payment.TransactionType = charge.TransactionType;
            payment.PaymentCardURL = DashboardURL + "/spei-pdf/" + MerchantId + "/" + charge.Id;
            payment.JsonData = charge.ToJson();

            return payment;

        }

        public PaymentModel CreateTDCPayment(PaymentModel payment)
        {
            OpenpayAPI openpayAPI = new OpenpayAPI(OpenpayKey, MerchantId);
            openpayAPI.Production = false;

            Customer customer = new Customer
            {
                Name = "Test ",
                LastName = "Client",
                Email = "test@ensitech.com"
            };

            Customer customerCreated = openpayAPI.CustomerService.Create(customer);

            ChargeRequest request = new ChargeRequest
            {
                Method = "card",
                SourceId = payment.TokenId,
                Amount = payment.Amount,
                OrderId = payment.OrderId,
                Description = "Pago TDC Prueba",
                DeviceSessionId = payment.DeviceSessionId,
                Customer = customer
            };

            // Opcional, si estamos usando puntos
            //request.UseCardPoints = false; //useCardPoints;

            Charge charge = openpayAPI.ChargeService.Create(request);

            payment.Id = charge.Id;
            payment.DueDate = request.DueDate;
            payment.Status = charge.Status;
            payment.TransactionType = charge.TransactionType;
            payment.PaymentCardURL = DashboardURL + "/spei-pdf/" + MerchantId + "/" + charge.Id;
            payment.JsonData = charge.ToJson();

            return payment;
        }
    }
}
