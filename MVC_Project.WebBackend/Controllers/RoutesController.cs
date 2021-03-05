using MVC_Project.Domain.Services;
using MVC_Project.WebBackend.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Project.WebBackend.Controllers
{
    public class RoutesController : BaseController
    {
        [HttpGet]
        [AllowAnonymous]
        public JsonResult GetRoutes(string cedis, string term, string page)
        {
            //TODO Get routes from service
            var routes = new List<RouteViewModel>
            {
                new RouteViewModel
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Ruta 01"
                },
                new RouteViewModel
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Ruta 02"
                },
                new RouteViewModel
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Ruta 03"
                },
            };

            var results = routes.Select(c => new SelectViewModel
            {
                id = c.Id,
                text = c.Name
            });

            return Json(new
            {
                results
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
