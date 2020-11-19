using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Models;
using Business.Models.Requests;
using Business.Models.Responses;

namespace Business.Services
{
    public interface IUserService
    {
        Task<UserModel> GetUser(string login);

        Task<LimitedUserModel> GetUserLimited(string login);

        Task<bool> UserExists(string login);

        Task ChangeUserInfo(ChangeUserInfo changeInfo);
    }
}