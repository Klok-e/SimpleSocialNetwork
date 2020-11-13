using System.Threading.Tasks;
using Business.Models;

namespace Business.Services
{
    public interface IUserService
    {
        public Task<UserModel> Get(string login);
    }
}