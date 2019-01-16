namespace MVC_Project.Domain.Services
{
    public interface IAuthService
    {
        bool Authenticate(string username, string password);

        string EncryptPassword(string password);
    }
}
