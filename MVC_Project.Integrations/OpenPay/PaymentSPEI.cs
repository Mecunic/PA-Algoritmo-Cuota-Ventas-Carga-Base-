using Openpay;
using Openpay.Entities;
using Openpay.Entities.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Project.Integrations.OpenPay
{
    public class PaymentSPEI
    {

        public string CreatePayment(string OrderId, decimal Amount)
        {
            OpenpayAPI openpayAPI = new OpenpayAPI("sk_3d9e93155b6f470ab1805ce800289b1c", "mxcdqesyvproizgrfuhg");
            openpayAPI.Production = false;


            Customer customer = new Customer();
            customer.Name = "Tet Client";
            customer.LastName = "C#";
            customer.Email = "test@ensitech.com";
            customer.Address = new Address();
            customer.Address.Line1 = "line 1";
            customer.Address.PostalCode = "12355";
            customer.Address.City = "Queretaro";
            customer.Address.CountryCode = "MX";
            customer.Address.State = "Queretaro";

            Customer customerCreated = openpayAPI.CustomerService.Create(customer);

            ChargeRequest request = new ChargeRequest
            {
                OrderId = OrderId,
                Amount = Amount,
                DueDate = DateTime.Now.AddDays(2),
                Method = "bank_account",
                Description = "Pago SPEI Prueba",
            };

            Charge charge = openpayAPI.ChargeService.Create(customerCreated.Id, request);

            return charge.ToJson();

        }

    }
}
