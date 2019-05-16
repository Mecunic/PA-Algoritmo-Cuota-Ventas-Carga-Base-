using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_Project.Resources;

namespace MVC_Project.Web.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult ShowError(string errorMessage, string signIn)
        {
            //ViewBag.SignIn = signIn;
            ViewBag.ErrorMessage = errorMessage;
            return View();
        }

        public ActionResult ShowErrorKey(string errorKey)
        {
            string errorMessage = ViewLabels.ResourceManager.GetString(errorKey);
            ViewBag.ErrorMessage = errorMessage;
            return View("ShowError");
        }
        
        public ActionResult Reauth(string redirectUri)
        {
            ViewBag.RedirectUri = redirectUri;
            return View();
        }

        public ViewResult Index()
        {
            TempData["ErrorMessage"] = "No es posible realizar la acción solicitada.";
            return View("GenericError");
        }

        public ViewResult PageNotFound()
        {
            TempData["ErrorMessage"] = "La página solicitada no existe.";
            return View("GenericError");
        }

        public ViewResult InternalError()
        {
            TempData["ErrorMessage"] = "Error de procesamiento interno en servidor.";
            return View("GenericError");
        }
    }
}