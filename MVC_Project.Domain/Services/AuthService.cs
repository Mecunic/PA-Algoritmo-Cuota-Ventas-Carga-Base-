using MVC_Project.Domain.Entities;
using MVC_Project.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Project.Domain.Services
{
    public class AuthService : IAuthService
    {
        private IRepository<User> _repository;

        public AuthService(IRepository<User> repository)
        {
            _repository = repository;
        }

        public bool Authenticate(string username, string password)
        {
            User user = _repository.FindBy(u => u.Email == username).FirstOrDefault();              
            return user != null && user.Password == password;
        }

        public string EncryptPassword(string password)
        {            
            throw new NotImplementedException();
        }
    }
}
