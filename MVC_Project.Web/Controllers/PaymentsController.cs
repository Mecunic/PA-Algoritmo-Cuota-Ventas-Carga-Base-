using MVC_Project.Domain.Entities;
using MVC_Project.Domain.Services;
using MVC_Project.Integrations.PaymentsOpenPay;
using MVC_Project.Utils;
using MVC_Project.Web.AuthManagement;
using MVC_Project.Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MVC_Project.Web.Controllers
{

    public class PaymentsController : BaseController
    {
        private PaymentService _paymentService;

        public PaymentsController(PaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [Authorize]
        public ActionResult CreateTDC()
        {
            PaymentViewModel model = new PaymentViewModel();

            return View(model);
        }

        [Authorize]
        public ActionResult CreateSPEI()
        {
            PaymentViewModel model = new PaymentViewModel();
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public ActionResult CreateSPEI(PaymentViewModel model)
        {
            OpenPayService paymentProviderService = new OpenPayService();
            PaymentModel payment = new PaymentModel()
            {
                OrderId = model.OrderId,
                Amount = model.Amount
            };

            payment = paymentProviderService.CreateSPEIPayment(payment);

            //Primero guardar en BD
            Payment paymentBO = new Payment();
            paymentBO.CreationDate = DateUtil.GetDateTimeNow();
            paymentBO.User = new User() { Id = Authenticator.AuthenticatedUser.Id };
            paymentBO.Amount = model.Amount;
            paymentBO.OrderId = model.OrderId;
            paymentBO.ProviderId = payment.Id;
            paymentBO.Status = payment.Status;
            paymentBO.DueDate = payment.DueDate;
            paymentBO.Method = "bank_account";
            paymentBO.TransactionType = "charge";

            paymentBO.ConfirmationDate = null;

            _paymentService.Create(paymentBO);

            model.Id = payment.Id;
            model.JsonData = payment.JsonData;
            model.DueDate = payment.DueDate;
            model.PaymentCardURL = payment.PaymentCardURL;

            return View("CreateSPEI", model);
        }

        [HttpPost]
        [Authorize]
        public ActionResult CreateTDC(PaymentViewModel model)
        {
            OpenPayService paymentProviderService = new OpenPayService();

            //Simulado
            model.OrderId = Guid.NewGuid().ToString().Substring(24);
            model.Amount = Convert.ToInt32( (new Random().NextDouble() * 10000) );

            PaymentModel payment = new PaymentModel()
            {
                OrderId = model.OrderId,
                Amount = model.Amount,
                TokenId = model.TokenId,
                DeviceSessionId = model.DeviceSessionId
            };

            //Primero en BD
            Payment paymentBO = new Payment();
            paymentBO.CreationDate = DateUtil.GetDateTimeNow();
            paymentBO.User = new User() { Id = Authenticator.AuthenticatedUser.Id };
            paymentBO.Amount = model.Amount;
            paymentBO.OrderId = model.OrderId;
            paymentBO.Status = "in_progress";
            paymentBO.Method = "card";
            paymentBO.TransactionType = "charge";

            paymentBO.ConfirmationDate = null;
            _paymentService.Create(paymentBO);

            //Luego cobrar
            payment = paymentProviderService.CreateTDCPayment(payment);

            //Luego actualizar
            paymentBO.ProviderId = payment.Id;
            paymentBO.Status = payment.Status;
            paymentBO.DueDate = payment.DueDate;
            _paymentService.Update(paymentBO);

            model.Id = payment.Id;
            model.JsonData = payment.JsonData;
            model.DueDate = payment.DueDate;
            model.PaymentCardURL = payment.PaymentCardURL;

            return View("CreateTDC", model);
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
                        System.Diagnostics.Trace.TraceInformation("\t\t Transaction Id: " + paymentEvent.transaction.id);
                        System.Diagnostics.Trace.TraceInformation("\t\t Order Id: " + paymentEvent.transaction.order_id);
                        System.Diagnostics.Trace.TraceInformation("\t\t Authorization: " + paymentEvent.transaction.authorization);

                        Payment payment = _paymentService.GetByOrderTransaction(paymentEvent.transaction.order_id, paymentEvent.transaction.id);
                        
                        if (payment!=null)
                        {
                            System.Diagnostics.Trace.TraceInformation("\t\t Payment BO ID: " + payment.Id);
                            payment.Status = paymentEvent.transaction.status;
                            payment.LogData = rawJSON;
                            if (paymentEvent.type == "charge.succeeded")
                            {
                                payment.ConfirmationDate = DateUtil.GetDateTimeNow();
                                payment.AuthorizationCode = paymentEvent.transaction.authorization;
                            }
                            _paymentService.Update(payment);
                            System.Diagnostics.Trace.TraceInformation("\t\t Payment BO Status Updated: " + payment.Status);
                        }
                    }
                }
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}