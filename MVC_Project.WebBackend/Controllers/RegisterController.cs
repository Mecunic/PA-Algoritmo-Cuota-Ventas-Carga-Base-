﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Project.WebBackend.Controllers
{
    public class RegisterController : BaseController
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }
    }
}