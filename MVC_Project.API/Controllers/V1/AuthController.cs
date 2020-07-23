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
                List<MessageResponse> messages = new List<MessageResponse>();
                var user = _authService.Authenticate(request.Username, SecurityUtil.EncryptPassword(request.Password));
                if (user == null || !user.Status)
                {
                    messages.Add(new MessageResponse { Type = MessageType.error.ToString("G"), Description = "El usuario no existe o contraseña inválida." });
                    return CreateErrorResponse(null, messages);
                }
                if (user.Role.Code != Constants.ROLE_DEFAULT_API)
                {
                    messages.Add(new MessageResponse { Type = MessageType.error.ToString("G"), Description = "El usuario no cuenta con acceso al API." });
                    return CreateErrorResponse(null, messages);
                }
                var expiration = DateTime.UtcNow.AddHours(Constants.HOURS_EXPIRATION_KEY);
                user.ApiKey = Guid.NewGuid().ToString();
                user.ExpiraApiKey = expiration;
                _userService.Update(user);
                var response = new AuthUserResponse
                {
                    ApiKey = user.ApiKey,
                    ApiKeyExpiration = expiration.ToString(Constants.DATE_FORMAT_CALENDAR),
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
                return CreateErrorResponse(e, null);
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
                messages.Add(new MessageResponse { Type = MessageType.error.ToString("G"), Description = "No se pudo crear la cuenta del usuario." });
            }
            return CreateResponse(messages);
        }
    }
}
