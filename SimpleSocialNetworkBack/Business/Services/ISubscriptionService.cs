using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Models.Responses;

namespace Business.Services
{
    public interface ISubscriptionService
    {
        Task<IEnumerable<SubscriptionModel>> GetUserSubscribedTo(string login);

        Task<bool> IsUserSubscribedTo(string login);

        Task SubscribeTo(string login);

        Task UnsubscribeFrom(string login);
    }
}