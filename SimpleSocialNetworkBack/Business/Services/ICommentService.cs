using System.Threading.Tasks;
using Business.Models.Requests;

namespace Business.Services
{
    public interface ICommentService
    {
        public Task CreateComment(string user, CreateCommentModel comment);
    }
}