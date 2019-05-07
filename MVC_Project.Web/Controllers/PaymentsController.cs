using MVC_Project.Integrations.OpenPay;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MVC_Project.Web.Controllers
{
    public class PaymentsController : Controller
    {
        [AllowAnonymous]
        public string Index(string OrderId, decimal Amount)
        {
            string JsonData = string.Empty;

            PaymentSPEI paymentSPEI = new PaymentSPEI();
            JsonData = paymentSPEI.CreatePayment(OrderId, Amount);

            return JsonData;
        }

        // GET: Payments
        [AllowAnonymous]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Tracking()
        {
            System.IO.StreamReader reader = new System.IO.StreamReader(HttpContext.Request.InputStream);
            string rawJSON = reader.ReadToEnd();
            System.Diagnostics.Trace.TraceInformation("PaymentsController [rawJSON] : " + rawJSON); // For debugging to the Azure Streaming logs

            PaymentEventModel paymentEvent = JsonConvert.DeserializeObject<PaymentEventModel>(rawJSON);
            if (paymentEvent != null)
            {
                if (!string.IsNullOrWhiteSpace(paymentEvent.type))
                {
                    System.Diagnostics.Trace.TraceInformation("\tTransaction: " + paymentEvent.transaction); 
                    if (paymentEvent.transaction != null)
                    {
                        System.Diagnostics.Trace.TraceInformation("\t\t Id: " + paymentEvent.transaction.id); 
                        System.Diagnostics.Trace.TraceInformation("\t\t Authorization: " + paymentEvent.transaction.authorization); 
                    }
                }
            }

            //List<PaymentEventModel> paymentEvents = JsonConvert.DeserializeObject<List<PaymentEventModel>>(rawJSON);

            //if (paymentEvents != null)
            //{
            //    string count = paymentEvents.Count.ToString();
            //    //string urlKey = Request.QueryString["k"];

            //    System.Diagnostics.Trace.TraceInformation("PaymentsController [count] : " + count);

            //    foreach (PaymentEventModel paymenEvent in paymentEvents)
            //    {
            //        if (!string.IsNullOrWhiteSpace(paymenEvent.type))
            //        {
            //            System.Diagnostics.Trace.TraceInformation("Transaction: " + paymenEvent.transaction); // For debugging to the Azure Streaming logs
            //        }
            //    }
            //}

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}