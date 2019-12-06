using MVC_Project.Domain.Entities;
using MVC_Project.Domain.Services;
using MVC_Project.Integrations.Payments;
using MVC_Project.Utils;
using MVC_Project.Web.AuthManagement;
using MVC_Project.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Project.Web.Controllers
{
    [AllowAnonymous]
    public class CheckoutController : BaseController
    {

        private IPaymentService _paymentService;
        private IUserService _userService;
        private IPaymentServiceProvider paymentProviderService;
        private int TransferExpirationDays;
        private bool UseSelective3DSecure;
        private string GlobalClientId;
        private string SecureVerificationURL;
        private string OpenpayWebhookKey;

        public CheckoutController(IPaymentService paymentService, IUserService userService)
        {
            _paymentService = paymentService;
            _userService = userService;
            TransferExpirationDays = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["Payments.TransferExpirationDays"]);
            UseSelective3DSecure = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["Payments.UseSelective3DSecure"]);
            GlobalClientId = System.Configuration.ConfigurationManager.AppSettings["Payments.OpenpayGeneralClientId"];
            SecureVerificationURL = System.Configuration.ConfigurationManager.AppSettings["Payments.SecureVerificationURL"];
            OpenpayWebhookKey = System.Configuration.ConfigurationManager.AppSettings["Payments.OpenpayWebhookKey"];
            paymentProviderService = new OpenPayService();
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Test()
        {
            //Simular datos
            String newOrderId = Guid.NewGuid().ToString().Substring(24);
            PaymentViewModel model = new PaymentViewModel
            {
                OrderId = newOrderId,
                Amount = Convert.ToInt32((new Random().NextDouble() * 10000)),
                Description = String.Format("Payment for Order Id # {0}", newOrderId)
            };
            return View("Test", model);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Index(PaymentViewModel model)
        {
            //TODO: Validar los paramestros del request
            return View("Index", model);
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProceedTDC(PaymentViewModel model)
        {

            PaymentModel payment = new PaymentModel()
            {
                ClientId = GlobalClientId,
                OrderId = model.OrderId,
                Amount = model.Amount,
                Description = model.Description,
                TokenId = model.TokenId,
                DeviceSessionId = model.DeviceSessionId,
                RedirectUrl = SecureVerificationURL,
                //Use3DSecure = true
            };

            //Primero en BD
            Payment paymentBO = new Payment();
            paymentBO.CreationDate = DateUtil.GetDateTimeNow();
            paymentBO.User = new User() { Id = 8 }; //TODO: Sacar el usuario del config
            paymentBO.Amount = model.Amount;
            paymentBO.OrderId = model.OrderId;
            paymentBO.Status = PaymentStatus.IN_PROGRESS;
            paymentBO.Method = PaymentMethod.CARD;
            paymentBO.TransactionType = PaymentType.CHARGE;

            paymentBO.ConfirmationDate = null;
            _paymentService.Create(paymentBO);

            //Luego cobrar
            model.PaymentMethod = PaymentMethod.CARD;
            payment = paymentProviderService.CreateTDCPayment(payment);

            //Si hubiera reintento, probar Antifraude
            if (UseSelective3DSecure && !payment.ChargeSuccess & payment.ResultCode == PaymentError.ANTI_FRAUD)
            {
                payment.Use3DSecure = true;
                payment = paymentProviderService.CreateTDCPayment(payment);
            }

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

            //
            Session.Add("Payments.PaymentModel", model);

            if (payment.PaymentMethod != null && !string.IsNullOrEmpty(payment.PaymentMethod.RedirectUrl))
            {
                return Redirect(payment.PaymentMethod.RedirectUrl);
            }

            return View("Test");
        }

        
    }
}