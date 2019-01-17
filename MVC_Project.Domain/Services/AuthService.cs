using MVC_Project.Domain.Entities;
using MVC_Project.Domain.Repositories;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace MVC_Project.Domain.Services
{
    public class AuthService : IAuthService
    {
        private IRepository<User> _repository;

        public AuthService(IRepository<User> repository)
        {
            _repository = repository;
        }

        public User Authenticate(string username, string password)
        {
            User user = _repository.FindBy(u => u.Email == username).FirstOrDefault();
            if (user != null && user.Password == password) return user;
            return null;
        }

        public string EncryptPassword(string password)
        {            
            using (SHA256 sha256Hash = SHA256.Create())  
            {                  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));  
                
                StringBuilder builder = new StringBuilder();  
                for (int i = 0; i < bytes.Length; i++)  
                {  
                    builder.Append(bytes[i].ToString("x2"));  
                }  
                return builder.ToString();  
            }  
        }
    }
}
