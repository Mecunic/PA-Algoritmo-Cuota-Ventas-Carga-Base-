using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Project.WebBackend.Controllers
{    
    public class FeesController : Controller
    {
        // GET: Fee
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Calculate()
        {
            return View();
        }

        public ActionResult Tracking()
        {
            return View();
        }

        public ActionResult Tweaking()
        {
            return View();
        }
    }
}