using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Models.Requests;
using Business.Models.Responses;

namespace Business.Services
{
    public interface IOpMessageService
    {
        Task<IEnumerable<OpMessageModel>> GetAll();

        Task<int> MakeAPost(CreateOpMessageModel model);

        Task<OpMessageModel> GetById(int id);

        Task<IEnumerable<CommentModel>> GetComments(int postId);

        Task<bool> PostExists(int postId);

        Task VotePost(VotePost votePost);

        Task<IEnumerable<OpMessageModel>> PostsFromUser(string login);
        Task DeletePostSoft(int postId);
    }
}