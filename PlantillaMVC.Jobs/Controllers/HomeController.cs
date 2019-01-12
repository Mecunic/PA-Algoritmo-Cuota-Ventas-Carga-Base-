﻿using PlantillaMVC.Domain.Services;
using PlantillaMVC.Integrations;
using PlantillaMVC.Integrations.Hubspot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PlantillaMVC.Jobs.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //IHubspotService apiService = new HubspotService();
            //string response = apiService.AssociateCompanyToTicket(1086248987, 3785340);
            //Trace.TraceInformation(response);

            IHubspotService apiService = new HubspotService();
            PipelinesHubSpotResult result = apiService.GetAllPipelines();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}