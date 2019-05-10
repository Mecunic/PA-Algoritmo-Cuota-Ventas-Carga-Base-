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
    public class PaymentSPEI
    {
        string OpenpayKey;
        string MerchantId;
        string DashboardURL;

        public PaymentSPEI()
        {
            OpenpayKey = System.Configuration.ConfigurationManager.AppSettings["Payments.OpenpayKey"];
            MerchantId = System.Configuration.ConfigurationManager.AppSettings["Payments.MerchantId"];
            DashboardURL = System.Configuration.ConfigurationManager.AppSettings["Payments.DashboardURL"];
        }

        public PaymentModel CreatePayment(PaymentModel payment)
        {
            OpenpayAPI openpayAPI = new OpenpayAPI(OpenpayKey, MerchantId);
            openpayAPI.Production = false;
            
            Customer customer = new Customer();
            customer.Name = "Tet Client";
            customer.LastName = "C#";
            customer.Email = "test@ensitech.com";
            //customer.Address = new Address();
            //customer.Address.Line1 = "line 1";
            //customer.Address.PostalCode = "12355";
            //customer.Address.City = "Queretaro";
            //customer.Address.CountryCode = "MX";
            //customer.Address.State = "Queretaro";

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

    }
}
