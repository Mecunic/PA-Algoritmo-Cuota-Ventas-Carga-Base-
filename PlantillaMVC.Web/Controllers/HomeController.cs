using HubSpot.NET.Api.Contact.Dto;
using HubSpot.NET.Api.Deal.Dto;
using HubSpot.NET.Api.Engagement.Dto;
using HubSpot.NET.Core;
using PlantillaMVC.Domain.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PlantillaMVC.Web.Controllers {
    public class HomeController : Controller {
        public ActionResult Index() {

            //IHubspotService service = new HubspotService();
            //var contact = service.CreateContact();

            var api = new HubSpotApi("3b609872-906e-42ae-92da-2e18ef841210");

            /*CREACION DE CONTACTOS*/
            //var contact = api.Contact.Create(new ContactHubSpotModel()
            //{
            //    Email = "john@squaredup.com",
            //    FirstName = "John",
            //    LastName = "Smith",
            //    Phone = "00000 000000",
            //    Company = "Squared Up Ltd."
            //});
            //Trace.TraceInformation(string.Format("{0}", contact));

            /*CREACION DE NOTAS*/
            //api.Engagement.Create(new EngagementHubSpotModel()
            //{
            //    Engagement = new EngagementHubSpotEngagementModel()
            //    {
            //        //String; One of NOTE, EMAIL, TASK, MEETING, or CALL, the type of the engagement.
            //        Type = "NOTE" //used for file attachments
            //    },
            //    Metadata = new
            //    {
            //        body = "This is an example note for a company"
            //    },
            //    Associations = new EngagementHubSpotAssociationsModel()
            //    {
            //        CompanyIds = new List<long>() { 1033124970 /*contact.Id.Value*/ }
            //        //ContactIds = new List<long>() { 151 /*contact.Id.Value*/ } //use the ID of the created contact from above
            //    },
            //});

            /*LEER DEALS*/
            //var dealList = api.Deal.List<DealHubSpotModel>(false,
            //        new ListRequestOptions(250) { PropertiesToInclude = new List<string> { "dealname", "amount" } });

            //foreach(var deal in dealList.Deals)
            //{
            //    Trace.TraceInformation(string.Format("{0} - {1}", deal.Name, deal.Amount));
            //}

            return View();
        }

        public ActionResult About() {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact() {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}