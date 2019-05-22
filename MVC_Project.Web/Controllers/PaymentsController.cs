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
        private UserService _userService;
        private int TransferExpirationDays;

        public PaymentsController(PaymentService paymentService, UserService userService)
        {
            _paymentService = paymentService;
            _userService = userService;
            TransferExpirationDays = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["Payments.TransferExpirationDays"]);
        }

        [Authorize]
        public ActionResult Index()
        {
            PaymentViewModel model = new PaymentViewModel();

            return View(model);
        }

        [Authorize]
        public ActionResult Checkout()
        {
            PaymentViewModel model = new PaymentViewModel();

            //Simular datos
            model.OrderId = Guid.NewGuid().ToString().Substring(24);
            model.Amount = Convert.ToInt32((new Random().NextDouble() * 10000));

            return View(model);
        }

        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult CheckoutProceed(PaymentViewModel model)
        {
            //Esto puede hacerse dinamico
            if (model.PaymentMethod == PaymentMethod.BANK_ACCOUNT)
            {
                model.DueDate = DateUtil.GetDateTimeNow().AddDays(TransferExpirationDays);
                return View("CreateSPEI", model);
            }
            if (model.PaymentMethod == PaymentMethod.CARD)
            {
                return View("CreateTDC", model);
            }
            return RedirectToAction("Index", "Error");
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult CreateSPEI(PaymentViewModel model)
        {
            OpenPayService paymentProviderService = new OpenPayService();
            PaymentModel payment = new PaymentModel()
            {
                ClientId = "avfwrv0q9x2binx9odgf",
                OrderId = model.OrderId,
                Amount = model.Amount,
                DueDate = DateUtil.GetDateTimeNow().AddDays(TransferExpirationDays),
                Description = String.Format("Payment for Order Id # {0}", model.OrderId),
            };

            payment = paymentProviderService.CreateBankTransferPayment(payment);
            model.ChargeSuccess = payment.ChargeSuccess;

            if (payment.ChargeSuccess)
            {
                //Primero guardar en BD
                Payment paymentBO = new Payment();
                paymentBO.CreationDate = DateUtil.GetDateTimeNow();
                paymentBO.User = new User() { Id = Authenticator.AuthenticatedUser.Id };
                paymentBO.Amount = model.Amount;
                paymentBO.OrderId = model.OrderId;
                paymentBO.ProviderId = payment.Id;
                paymentBO.Status = payment.Status;
                paymentBO.DueDate = payment.DueDate;
                paymentBO.Method = PaymentMethod.BANK_ACCOUNT;
                paymentBO.TransactionType = PaymentType.CHARGE;

                paymentBO.ConfirmationDate = null;

                _paymentService.Create(paymentBO);

                model.Id = payment.Id;
                model.Description = payment.Description;
                model.JsonData = payment.ResultData;
                model.DueDate = payment.DueDate;
                model.PaymentCardURL = payment.PaymentCardURL;
                model.BankName = payment.PaymentMethod.BankName;
                model.Clabe = payment.PaymentMethod.Clabe;
                model.Reference = payment.PaymentMethod.Reference;
                model.Name = payment.PaymentMethod.Name;
            }
            else
            {
                model.Description = payment.ResultData;
            }

            //return View("CreateSPEI", model);
            return View("CheckoutSuccess", model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTDC(PaymentViewModel model)
        {
            OpenPayService paymentProviderService = new OpenPayService();

            PaymentModel payment = new PaymentModel()
            {
                ClientId = "avfwrv0q9x2binx9odgf",
                OrderId = model.OrderId,
                Amount = model.Amount,
                TokenId = model.TokenId,
                DeviceSessionId = model.DeviceSessionId,
                Description = String.Format("Payment for Order Id # {0}", model.OrderId),
                Use3DSecure = true,
                RedirectUrl = "http://localhost:52222/Payments/SecureVerification"
            };

            //Primero en BD
            Payment paymentBO = new Payment();
            paymentBO.CreationDate = DateUtil.GetDateTimeNow();
            paymentBO.User = new User() { Id = Authenticator.AuthenticatedUser.Id };
            paymentBO.Amount = model.Amount;
            paymentBO.OrderId = model.OrderId;
            paymentBO.Status = PaymentStatus.IN_PROGRESS;
            paymentBO.Method = PaymentMethod.CARD;
            paymentBO.TransactionType = PaymentType.CHARGE;

            paymentBO.ConfirmationDate = null;
            _paymentService.Create(paymentBO);

            //Luego cobrar
            payment = paymentProviderService.CreateTDCPayment(payment);
            
            model.ChargeSuccess = payment.ChargeSuccess;

            if (payment.ChargeSuccess)
            {
                //Luego actualizar
                paymentBO.ProviderId = payment.Id;
                paymentBO.Status = payment.Status;
                paymentBO.DueDate = payment.DueDate;
                paymentBO.LogData = payment.ResultData;
                _paymentService.Update(paymentBO);

                model.Id = payment.Id;
                model.Description = payment.Description;
                model.JsonData = payment.ResultData;
                model.DueDate = payment.DueDate;
                model.PaymentCardURL = payment.PaymentCardURL;
            }
            else
            {
                paymentBO.Status = PaymentStatus.ERROR;
                paymentBO.LogData = payment.ResultData;
                _paymentService.Update(paymentBO);
                model.Description = payment.ResultData;
            }

            if (payment.PaymentMethod != null && !string.IsNullOrEmpty(payment.PaymentMethod.RedirectUrl))
            {
                return Redirect(payment.PaymentMethod.RedirectUrl);
            }

            //return View("CreateTDC", model);
            return View("CheckoutSuccess", model);
        }


        [AllowAnonymous]
        [ValidateInput(false)]
        public ActionResult SecureVerification(string id)
        {
            //Payment payment = _paymentService.GetByOrderTransaction(id, null);
            
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
        
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
                        User user = _userService.GetById(payment.User.Id);
                        
                        if (payment!=null)
                        {
                            System.Diagnostics.Trace.TraceInformation("\t\t Payment BO ID: " + payment.Id);
                            payment.Status = paymentEvent.transaction.status;
                            payment.LogData = rawJSON;
                            if (paymentEvent.type == PaymentEventStatus.CHARGE_SUCCEEDED)
                            {
                                payment.ConfirmationDate = DateUtil.GetDateTimeNow();
                                payment.AuthorizationCode = paymentEvent.transaction.authorization;

                                Dictionary<string, string> customParams = new Dictionary<string, string>();
                                customParams.Add("param1", user.FirstName);
                                customParams.Add("param2", paymentEvent.transaction.order_id);
                                customParams.Add("param3", payment.AuthorizationCode);
                                NotificationUtil.SendNotification(user.Email, customParams, Constants.NOT_TEMPLATE_CHARGESUCCESS);
                            }
                            _paymentService.Update(payment);
                            System.Diagnostics.Trace.TraceInformation("\t\t Payment BO Status Updated: " + payment.Status);
                        }
                    }
                }
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        //[Authorize]
        //public ActionResult CreatePayment(PaymentViewModel model)
        //{
        //    OpenPayService paymentProviderService = new OpenPayService();
        //    PaymentModel payment = new PaymentModel()
        //    {
        //        ClientId = "avfwrv0q9x2binx9odgf",
        //        OrderId = model.OrderId,
        //        Amount = model.Amount,
        //        Description = String.Format("Payment for Order Id # {0}", model.OrderId),
        //        TokenId = model.TokenId,
        //        DeviceSessionId = model.DeviceSessionId
        //    };
        //    model.ChargeSuccess = false;

        //    #region Pagos con SPEI
        //    if (model.PaymentMethod == PaymentMethod.BANK_ACCOUNT)
        //    {
        //        payment = paymentProviderService.CreateBankTransferPayment(payment);
        //        model.ChargeSuccess = payment.ChargeSuccess;
        //        if (payment.ChargeSuccess)
        //        {
        //            //Primero guardar en BD
        //            Payment paymentBO = new Payment();
        //            paymentBO.CreationDate = DateUtil.GetDateTimeNow();
        //            paymentBO.User = new User() { Id = Authenticator.AuthenticatedUser.Id };
        //            paymentBO.Amount = model.Amount;
        //            paymentBO.OrderId = model.OrderId;
        //            paymentBO.ProviderId = payment.Id;
        //            paymentBO.Status = payment.Status;
        //            paymentBO.DueDate = payment.DueDate;
        //            paymentBO.Method = PaymentMethod.BANK_ACCOUNT;
        //            paymentBO.TransactionType = PaymentType.CHARGE;

        //            paymentBO.ConfirmationDate = null;

        //            _paymentService.Create(paymentBO);

        //            model.Id = payment.Id;
        //            model.Description = payment.Description;
        //            model.JsonData = payment.ResultData;
        //            model.DueDate = payment.DueDate;
        //            model.PaymentCardURL = payment.PaymentCardURL;
        //            model.BankName = payment.PaymentMethod.BankName;
        //            model.Clabe = payment.PaymentMethod.Clabe;
        //            model.Reference = payment.PaymentMethod.Reference;
        //            model.Name = payment.PaymentMethod.Name;
        //            model.Agreement = payment.PaymentMethod.Agreement;
        //        }

        //    }
        //    #endregion

        //    #region Pagos con Tarjeta
        //    if (model.PaymentMethod == PaymentMethod.CARD)
        //    {
        //        //Primero en BD
        //        Payment paymentBO = new Payment();
        //        paymentBO.CreationDate = DateUtil.GetDateTimeNow();
        //        paymentBO.User = new User() { Id = Authenticator.AuthenticatedUser.Id };
        //        paymentBO.Amount = model.Amount;
        //        paymentBO.OrderId = model.OrderId;
        //        paymentBO.Status = PaymentStatus.IN_PROGRESS;
        //        paymentBO.Method = PaymentMethod.CARD;
        //        paymentBO.TransactionType = PaymentType.CHARGE;

        //        paymentBO.ConfirmationDate = null;
        //        _paymentService.Create(paymentBO);

        //        //Luego cobrar
        //        payment = paymentProviderService.CreateTDCPayment(payment);
        //        model.ChargeSuccess = payment.ChargeSuccess;

        //        if (payment.ChargeSuccess)
        //        {
        //            //Luego actualizar
        //            paymentBO.ProviderId = payment.Id;
        //            paymentBO.Status = payment.Status;
        //            paymentBO.DueDate = payment.DueDate;
        //            paymentBO.LogData = payment.ResultData;
        //            _paymentService.Update(paymentBO);

        //            model.Id = payment.Id;
        //            model.Description = payment.Description;
        //            model.JsonData = payment.ResultData;
        //            model.DueDate = payment.DueDate;
        //            model.PaymentCardURL = payment.PaymentCardURL;
        //        }
        //        else
        //        {
        //            paymentBO.Status = PaymentStatus.ERROR;
        //            paymentBO.LogData = payment.ResultData;
        //            _paymentService.Update(paymentBO);
        //            model.Description = payment.ResultData;
        //        }
        //    }
        //    #endregion

        //    if (!model.ChargeSuccess)
        //    {
        //        model.Description = payment.ResultData;
        //    }

        //    return View("CheckoutSuccess", model);
        //}


        //[Authorize]
        //[ValidateAntiForgeryToken]
        //[HttpGet]
        //public ActionResult CreateTDC()
        //{
        //    PaymentViewModel model = new PaymentViewModel();

        //    return View(model);
        //}

        //[Authorize]
        //[HttpGet]
        //public ActionResult CreateSPEI()
        //{
        //    PaymentViewModel model = new PaymentViewModel();
        //    model.OrderId = Guid.NewGuid().ToString().Substring(24);
        //    return View(model);
        //}

        // GET: Payments

    }
}