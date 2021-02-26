using MVC_Project.WebBackend.Models;
using System;
using System.Web.Mvc;

namespace MVC_Project.WebBackend.Controllers
{
    public class StorageController : Controller
    {
        public ActionResult Index()
        {
            UserImportViewModel model = new UserImportViewModel();
            return View(model);
        }
    }
}