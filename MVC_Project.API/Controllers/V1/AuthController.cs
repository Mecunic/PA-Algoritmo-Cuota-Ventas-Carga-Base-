using Microsoft.Web.Http;
using MVC_Project.API.Models.Api_Request;
using MVC_Project.API.Models.Api_Response;
using MVC_Project.Domain.Entities;
using MVC_Project.Domain.Services;
using MVC_Project.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
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
        private IUserService _userService;
        private IRoleService _roleService;
        private IAuthService _authService;

        public AuthController(IUserService userService, IRoleService roleService, IAuthService authService)
        {
            _userService = userService;
            _roleService = roleService;
            _authService = authService;
        }

        [HttpPost]
        [Route("login")]
        public HttpResponseMessage Login([FromBody] LoginRequest request)
        {
            try
            {
                if (request == null)
                {
                    return CreateErrorResponse(HttpStatusCode.BadRequest, "No se recibieron los parámetros de entrada.");
                }
                var user = _authService.Authenticate(request.Username, request.Password);
                if (user == null)
                {
                    return CreateErrorResponse(HttpStatusCode.BadRequest, "El usuario no existe o contraseña inválida.");
                }
                if (!user.Status)
                {
                    return CreateErrorResponse(HttpStatusCode.BadRequest, "La cuenta del usuario se encuentra inactiva o no se ha confirmado.");
                }
                /*if (user.Role.Code != Constants.ROLE_DEFAULT_API)
                {
                    return CreateErrorResponse(HttpStatusCode.BadRequest, "El usuario no cuenta con acceso al API");
                }*/
                var expiration = DateUtil.GetDateTimeNow().AddHours(Constants.HOURS_EXPIRATION_KEY).ToUniversalTime();
                user.ApiKey = Guid.NewGuid().ToString();
                user.ExpiraApiKey = expiration;
                user.LastLoginAt = DateUtil.GetDateTimeNow();
                _userService.Update(user);
                var response = new AuthUserResponse
                {
                    ApiKey = user.ApiKey,
                    ApiKeyExpiration = DateUtil.ConvertToUnixTime(expiration),
                    Uuid = user.Uuid,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    MobilePhone = user.MobileNumber,
                };
                return CreateResponse(response);
            }
            catch (Exception e)
            {
                return CreateErrorResponse(e);
            }
        }

        [HttpGet]
        [Route("")]
        [AuthorizeApiKey]
        public HttpResponseMessage Get()
        {
            List<MessageResponse> messages = new List<MessageResponse>();
            int UserId = GetUserId();
            var user = _userService.GetById(UserId);
            if (user != null)
            {
                var response = new AuthUserResponse
                {
                    Uuid = user.Uuid,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    MobilePhone = user.MobileNumber,
                };
                return CreateResponse(response, "Datos devueltos correctamente");
            }
            else
            {
                return CreateErrorResponse(HttpStatusCode.BadRequest, "El usuario no existe o cuenta inválida.");   
            }
        }
    }
}
