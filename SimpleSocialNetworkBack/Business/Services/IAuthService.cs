using System.Threading.Tasks;
using Business.Models.Responses;

namespace Business.Services
{
    public interface IAuthService
    {
        Task<UserModel> Register(string login, string password);

        Task<LoggedInUser> Login(string login, string password);
    }
}
