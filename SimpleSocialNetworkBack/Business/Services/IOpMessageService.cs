using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Models;

namespace Business.Services
{
    public interface IOpMessageService
    {
        Task<IEnumerable<OpMessageModel>> GetAll();

        public Task<OpMessageModel> MakeAPost(UserModel user, CreateOpMessageModel model);
    }
}