using MVC_Project.Domain.Services;
using MVC_Project.Utils;
using MVC_Project.Web.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Project.Web.Controllers
{
    public class CalendarController : BaseController
    {
        IEventService _eventService;

        public CalendarController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet, Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet, Authorize]
        public JsonResult GetAllByFilter(string start, string end)
        {
            try
            {
                //NameValueCollection filtersValues = HttpUtility.ParseQueryString(filtros);
                var results = _eventService.GetAll();
                IList<EventData> dataResponse = new List<EventData>();
                foreach (var eventBO in results)
                {
                    EventData eventData = new EventData();
                    eventData.Id = eventBO.Id;
                    eventData.Uuid = eventBO.Uuid;
                    eventData.Title = eventBO.Title;
                    eventData.Description = eventBO.Description;
                    eventData.Start = eventBO.StartDate.ToString(Constants.DATE_FORMAT_CALENDAR);
                    eventData.End = eventBO.EndDate.ToString(Constants.DATE_FORMAT_CALENDAR);
                    eventData.IsFullDay = eventBO.IsFullDay;
                    dataResponse.Add(eventData);
                }
                return Json(dataResponse, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return new JsonResult
                {
                    Data = new { Mensaje = new { title = "Error", message = ex.Message } },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    MaxJsonLength = Int32.MaxValue
                };
            }
        }
    }
}