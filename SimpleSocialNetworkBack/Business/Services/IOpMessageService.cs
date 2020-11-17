using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Models;
using Business.Models.Requests;
using Business.Models.Responses;

namespace Business.Services
{
    public interface IOpMessageService
    {
        Task<IEnumerable<OpMessageModel>> GetAll();

        Task<int> MakeAPost(string user, CreateOpMessageModel model);

        Task<OpMessageModel> GetById(int id);

        Task<IEnumerable<CommentModel>> GetComments(int postId);

        Task<bool> PostExists(int postId);

        Task VotePost(VotePost votePost);
    }
}