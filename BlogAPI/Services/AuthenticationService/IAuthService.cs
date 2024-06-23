
using BlogAPI.Models;

namespace BlogAPI.Services
{
    public interface IAuthService
    {
        Task<(int, dynamic)> Registration(RegistrationModel model, string role);
        Task<(int, dynamic)> Login(LoginModel model);

    }
}

