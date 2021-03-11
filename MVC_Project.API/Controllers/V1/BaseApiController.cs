using MVC_Project.API.Models;
using MVC_Project.API.Models.Api_Response;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using System.Web.Mvc;

namespace MVC_Project.API.Controllers
{
    public class BaseApiController : ApiController
    {
        public int GetUserId()
        {
            return Convert.ToInt32(Thread.CurrentPrincipal.Identity.Name);
        }
        public HttpResponseMessage CreateResponse<T>(T data, string message = ""/*, PaginationResponse Pagination = null*/) where T : class
        {
            var response = new ApiResponse<T>()
            {
                Success = true,
                Result = "success",
                ResponseData = data,
                StatusCode = (int)HttpStatusCode.OK,
                Message = message,
                //Pagination = Pagination
            };
            //LOG de respuesta si es necesario
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        public HttpResponseMessage CreateErrorResponse(HttpStatusCode statusCode, string message)
        {
            int Status = Convert.ToInt32(statusCode);

            var response = new ApiResponse<MessageResponse>()
            {
                Success = false,
                Result = "error",
                ResponseData = new MessageResponse() { Description = message },
                StatusCode = (int)HttpStatusCode.BadRequest,
                Message = message
            };
            return Request.CreateResponse((HttpStatusCode)response.StatusCode, response);
            //return Request.CreateResponse(defaultCode, apiResponse, JsonUtil.Formatter(), JsonUtil.mediatype);
        }

        public HttpResponseMessage CreateErrorResponse(Exception exception)
        {
            string errorMsg = "Ha ocurrido un error!";
            int StatusCode = (int)HttpStatusCode.InternalServerError;
            if (exception != null)
            {
                Win32Exception win32Ex = exception as Win32Exception;
                errorMsg = exception.InnerException != null ? exception.InnerException.Message : exception.Message;
                StatusCode = win32Ex == null ? (int)HttpStatusCode.BadRequest : (int)HttpStatusCode.InternalServerError;
            }

            var response = new ApiResponse<MessageResponse>()
            {
                Success = false,
                Result = "error",
                ResponseData = new MessageResponse() { Description = errorMsg },
                StatusCode = StatusCode,
                Message = errorMsg
            };
            return Request.CreateResponse((HttpStatusCode)response.StatusCode, response);
        }


    }
}
