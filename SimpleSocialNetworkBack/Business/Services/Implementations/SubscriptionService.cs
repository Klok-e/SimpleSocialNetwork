using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Common;
using Business.Models.Responses;
using Business.Validation;
using DataAccess;
using DataAccess.Entities;

namespace Business.Services.Implementations
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly SocialDbContext _context;
        private readonly IMapper _mapper;
        private readonly TypedClaimsPrincipal _principal;

        public SubscriptionService(SocialDbContext context, IMapper mapper, TypedClaimsPrincipal principal)
        {
            _context = context;
            _mapper = mapper;
            _principal = principal;
        }

        #region ISubscriptionService Members

        public async Task<IEnumerable<SubscriptionModel>> GetUserSubscribedTo(string login)
        {
            var user = await _context.Users.FindAsync(login);
            ExceptionHelper.CheckEntitySoft(user, "user");

            return user.Subscriptions.Where(x => !x.IsNotActive && x.Target != null && !x.Target.IsDeleted)
                       .Select(x => _mapper.Map<Subscription, SubscriptionModel>(x));
        }

        public async Task<bool> IsUserSubscribedTo(string login)
        {
            var subscriberLogin = _principal.Name;
            if (subscriberLogin == login)
                // can't subscribe to self so always false
                return false;

            var user = await _context.Users.FindAsync(subscriberLogin);
            ExceptionHelper.CheckSelfSoft(user, "user");

            return user.Subscriptions.Where(x => !x.IsNotActive)
                       .Any(x => x.Target != null && x.Target.Login == login);
        }

        public async Task SubscribeTo(string login)
        {
            var (subscriber, target) = await SubscriberAndTarget(_principal.Name, login);

            var subs = subscriber.Subscriptions.Where(x => x.Target != null && x.Target.Login == target.Login)
                                 .ToList();

            // TODO: make sub and target into keys instead
            if (subs.Count > 1)
                throw new NotImplementedException();

            if (subs.Count == 0)
                await _context.Subscriptions.AddAsync(new Subscription { Subscriber = subscriber, Target = target });
            else
                subs[0]
                    .IsNotActive = false;

            await _context.SaveChangesAsync();
        }

        public async Task UnsubscribeFrom(string login)
        {
            var (subscriber, target) = await SubscriberAndTarget(_principal.Name, login);

            var subs = subscriber.Subscriptions.Where(x => x.Target != null && x.Target.Login == target.Login)
                                 .ToList();

            // TODO: make sub and target into keys instead
            if (subs.Count > 1)
                throw new NotImplementedException();

            // no subscriptions to this target so nothing to unsub from
            if (subs.Count == 0)
                return;

            subs[0]
                .IsNotActive = true;

            await _context.SaveChangesAsync();
        }

        #endregion

        private async Task<(ApplicationUser subscriber, ApplicationUser target)> SubscriberAndTarget(string? subscriber,
            string target)
        {
            if (subscriber == target)
                throw new ValidationException("Can't subscribe to self");

            var user = await _context.Users.FindAsync(subscriber);
            var userSubTo = await _context.Users.FindAsync(target);

            ExceptionHelper.CheckSelfSoft(user, "user");
            ExceptionHelper.CheckEntitySoft(userSubTo, "subscription target");

            return (user, userSubTo);
        }
    }
}
