using System;
using System.Net.Http;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVC_Project.API.Controllers.V1;
using MVC_Project.API.Models.Api_Request;
using MVC_Project.Data.Helpers;
using MVC_Project.Data.Repositories;
using MVC_Project.Data.Services;
using MVC_Project.Domain.Entities;
using MVC_Project.Domain.Helpers;
using MVC_Project.Domain.Services;
using MVC_Project.Utils;

namespace MVC_Project.API.Tests.Controllers
{
    [TestClass]
    public class AuthControllerTest
    {
        
        AuthController authController;

        public AuthControllerTest()
        {
            IUnitOfWork uof = new UnitOfWork();
            IUserService _userService = new UserService(new Repository<User>(uof));
            IRoleService _roleService = new RoleService(new Repository<Role>(uof));
            IAuthService _authService = new AuthService(new Repository<User>(uof));
            authController = new AuthController(_userService, _roleService, _authService)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };
        }
        [TestMethod]
        public void Login()
        {
            LoginRequest request = new LoginRequest();
            request.Username = "appuser@mail.com";
            request.Password = SecurityUtil.EncryptPassword("12345678");
            var result = authController.Login(request);
            Assert.IsNotNull(result);
            Assert.AreEqual (200,(int)result.StatusCode);
        }
    }
}
