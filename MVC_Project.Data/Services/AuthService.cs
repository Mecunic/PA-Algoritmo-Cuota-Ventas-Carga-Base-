using MVC_Project.Domain.Entities;
using MVC_Project.Domain.Repositories;
using MVC_Project.Domain.Services;
using MVC_Project.WebApis.Servicios;
using System.Linq;

namespace MVC_Project.Data.Services
{
    public class AuthService : IAuthService
    {
        private IRepository<User> _repository;

        public AuthService(IRepository<User> repository)
        {
            _repository = repository;
        }

        public User Authenticate(string username, string password, int cedis)
        {
            var response = InventariosService.Login(username, password, cedis);
            User user = _repository.FindBy(u => u.Email == response.Result.Username).FirstOrDefault();
            if (user != null) return user;
            return null;
        }
    }
}
