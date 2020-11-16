using System.Threading.Tasks;
using Business.Models;
using Business.Models.Responses;

namespace Business.Services
{
    public interface IUserService
    {
        public Task<UserModel> Get(string login);

        public Task<bool> UserExists(string login);
    }
}