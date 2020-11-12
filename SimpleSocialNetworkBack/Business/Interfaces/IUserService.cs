using System.Threading.Tasks;
using Business.Models;

namespace Business.Interfaces
{
    public interface IUserService
    {
        Task<UserModel> Register(string login, string password);
    }
}