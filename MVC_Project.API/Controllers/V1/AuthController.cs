using Microsoft.Web.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MVC_Project.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [RoutePrefix("api/v{version:apiVersion}/auth")]
    public class AuthController : BaseApiController
    {
        [HttpPost]
        [Route("login")]
        public HttpResponseMessage Login()
        {
            try
            {
                return CreateResponse("Success");
            }
            catch(Exception e)
            {
                return CreateErrorResponse(e, null);
            }
        }

        [HttpPost]
        [Route("register")]
        public HttpResponseMessage Register()
        {
            try
            {
                return CreateResponse("Success");
            }
            catch (Exception e)
            {
                return CreateErrorResponse(e, null);
            }
        }
    }
}
