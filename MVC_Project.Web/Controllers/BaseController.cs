using MVC_Project.Domain.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Unity.Attributes;

namespace MVC_Project.Web.Controllers
{
    public class BaseController : Controller
    {
        [Dependency]
        public IUnitOfWork UnitOfWork { get; set; }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.IsChildAction)
                UnitOfWork.BeginTransaction();
        }

        protected override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            if (!filterContext.IsChildAction)
                UnitOfWork.Commit();
        }

        protected JsonResult JsonStatusGone(string message)
        {
            Response.StatusCode = (int)System.Net.HttpStatusCode.Gone;
            return Json(new
            {
                Message = message
            }, JsonRequestBehavior.AllowGet);
        }
    }
}