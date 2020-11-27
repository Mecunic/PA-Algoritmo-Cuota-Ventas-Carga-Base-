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

namespace MVC_Project.API.Controllers
{
    [ApiVersion("1.0")]
    [RoutePrefix("api/v{version:apiVersion}/account")]
    public class AccountController : BaseApiController
    {
        private IUserService _userService;
        private IRoleService _roleService;
        private IAuthService _authService;

        public AccountController(IUserService userService, IRoleService roleService, IAuthService authService)
        {
            _userService = userService;
            _roleService = roleService;
            _authService = authService;
        }
        [HttpPost]
        [Route("register")]
        public HttpResponseMessage Register([FromBody] RegisterRequest request)
        {
            try
            {
                if (request == null)
                {
                    return CreateErrorResponse(HttpStatusCode.BadRequest, "No se recibieron los parámetros de entrada.");
                }
                var currentUsers = _userService.FindBy(x => x.Email.ToUpper().Trim() == request.Email.ToUpper().Trim());
                if (currentUsers.Count() > 0)
                {
                    return CreateErrorResponse(HttpStatusCode.BadRequest, "El correo electrónico proporcionado ya se encuentra registrado.");
                }
                /*if (!FormatUtil.IsEmailValid(request.Email.Trim()))
                {
                    return CreateErrorResponse(HttpStatusCode.BadRequest, "El formato de correo electrónico no es válido.");
                }*/
                
                Role roleBO = _roleService.FindBy(x => x.Code == Constants.ROLE_DEFAULT_API).FirstOrDefault();
                User user = new User
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Username = request.Email.Trim(), //DEFAULT
                    Email = request.Email.Trim(),
                    MobileNumber = request.MobileNumber,
                    Password = SecurityUtil.EncryptPassword(request.Password),
                    Uuid = Guid.NewGuid().ToString(),
                    Role = roleBO,
                    Permissions = roleBO.Permissions.ToList(),
                    CreatedAt = DateUtil.GetDateTimeNow(),
                    UpdatedAt = DateUtil.GetDateTimeNow(),
                    Status = true
                };
                _userService.Create(user);
                if (user.Id > 0)
                {
                    var response = new AuthUserResponse
                    {
                        Uuid = user.Uuid,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        MobilePhone = user.MobileNumber,
                    };
                    return CreateResponse(response, "La cuenta del usuario se creó correctamente");
                }
                else
                {
                    return CreateErrorResponse(HttpStatusCode.BadRequest, "No se pudo crear la cuenta del usuario.");
                }
            }
            catch (Exception e)
            {
                return CreateErrorResponse(e);
            }
        }

        [HttpGet]
        [Route("")]
        [AuthorizeApiKey]
        public HttpResponseMessage GetAccount()
        {
            int UserId = GetUserId();
            var user = _userService.GetById(UserId);
            if (user != null)
            {
                var response = new AuthUserResponse
                {
                    //ApiKey = user.ApiKey,
                    //ApiKeyExpiration = user.ExpiraApiKey,
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
                return CreateErrorResponse(HttpStatusCode.BadRequest, "No se pudo obtener los datos del usuario.");
            }
        }

        [HttpGet]
        [Route("recover")]
        public HttpResponseMessage Recover([FromUri(Name = "email")] string email)
        {
            try
            {
                var user = _userService.FindBy(e => e.Email == email).FirstOrDefault();
                if (user == null)
                {
                    return CreateErrorResponse(HttpStatusCode.BadRequest, "El correo electrónico solicitado no se encuentra registrado.");
                }
                if (user.Role.Code != Constants.ROLE_DEFAULT_API)
                {
                    return CreateErrorResponse(HttpStatusCode.BadRequest, "El usuario no cuenta con acceso al API");
                }
                string token = (user.Uuid + "@" + DateTime.Now.AddDays(1).ToString());
                token = EncryptorText.DataEncrypt(token).Replace("/", "!!").Replace("+", "$");
                List<string> Email = new List<string>();
                Email.Add(user.Email);
                Dictionary<string, string> customParams = new Dictionary<string, string>();
                string urlAccion = ConfigurationManager.AppSettings["_UrlServerAccess"].ToString();
                string link = urlAccion + "Auth/AccedeToken?token=" + token;
                customParams.Add("param1", user.Email);
                customParams.Add("param2", link);
                string template = "aa61890e-5e39-43c4-92ff-fae95e03a711";
                NotificationUtil.SendNotification(Email, customParams, template);

                user.ExpiraToken = DateUtil.GetDateTimeNow().AddDays(1);
                user.Token = token;
                _userService.Update(user);

                return CreateResponse("OK", "Datos devueltos correctamente");
            }
            catch (Exception e)
            {
                return CreateErrorResponse(e);
            }
        }

        [HttpPost]
        [Route("reset")]
        public HttpResponseMessage ResetPass(ResetPassRequest request)
        {
            try
            {
                var decrypted = EncryptorText.DataDecrypt(request.Token.Replace("!!", "/").Replace("$", "+"));
                if (string.IsNullOrEmpty(request.Token) || string.IsNullOrEmpty(decrypted))
                {
                    return CreateErrorResponse(HttpStatusCode.BadRequest, "Token de recuperación no encontrado.");
                }
                string id = decrypted.Split('@').First();
                var user = _userService.FindBy(x => x.Uuid == id).First();
                if (user == null || DateUtil.GetDateTimeNow() > user.ExpiraToken)
                {
                    return CreateErrorResponse(HttpStatusCode.BadRequest, "El token ha expirado.");
                }
                if (user.Role.Code != Constants.ROLE_DEFAULT_API)
                {
                    return CreateErrorResponse(HttpStatusCode.BadRequest, "El usuario no cuenta con acceso.");
                }
                user.Password = request.Password;
                _userService.Update(user);

                return CreateResponse("OK", "Datos actualizados correctamente");
            }
            catch (Exception e)
            {
                return CreateErrorResponse(e);
            }
        }
    }
}
