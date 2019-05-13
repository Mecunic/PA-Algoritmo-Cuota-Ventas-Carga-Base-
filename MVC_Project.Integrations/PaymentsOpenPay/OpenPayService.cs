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
            try
            {
                Customer customer = openpayAPI.CustomerService.Get(payment.ClientId);

                ChargeRequest request = new ChargeRequest
                {
                    OrderId = payment.OrderId,
                    Amount = payment.Amount,
                    DueDate = DateTime.Now.AddDays(2),
                    Method = PaymentMethod.Bank_Account,
                    Description = payment.Description,
                    Customer = customer
                };
            
                Charge charge = openpayAPI.ChargeService.Create(request);

                payment.Id = charge.Id;
                payment.DueDate = request.DueDate;
                payment.Status = charge.Status;
                payment.TransactionType = charge.TransactionType;
                payment.PaymentCardURL = DashboardURL + "/spei-pdf/" + MerchantId + "/" + charge.Id;
                payment.ResultData = charge.ToJson();
                payment.ChargeSuccess = true;
            }
            catch (OpenpayException ex)
            {
                payment.ChargeSuccess = false;
                payment.ResultData = ex.Description;
                payment.ResultCategory = ex.Category;
            }

            return payment;

        }

        public PaymentModel CreateTDCPayment(PaymentModel payment)
        {
            OpenpayAPI openpayAPI = new OpenpayAPI(OpenpayKey, MerchantId);
            openpayAPI.Production = false;

            try
            {

                Customer customer = openpayAPI.CustomerService.Get(payment.ClientId);

                ChargeRequest request = new ChargeRequest
                {
                    Method = PaymentMethod.Card,
                    SourceId = payment.TokenId,
                    Amount = payment.Amount,
                    OrderId = payment.OrderId,
                    Description = payment.Description,
                    DeviceSessionId = payment.DeviceSessionId,
                    Customer = customer
                };
                
                Charge charge = openpayAPI.ChargeService.Create(request);

                payment.Id = charge.Id;
                payment.DueDate = request.DueDate;
                payment.Status = charge.Status;
                payment.TransactionType = charge.TransactionType;
                payment.PaymentCardURL = DashboardURL + "/spei-pdf/" + MerchantId + "/" + charge.Id;
                payment.ResultData = charge.ToJson();

            }
            catch (OpenpayException ex)
            {
                payment.ChargeSuccess = false;
                payment.ResultData = ex.Description;
                payment.ResultCategory = ex.Category;
            }

            return payment;
        }
    }
}
