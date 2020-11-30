using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business;
using Business.Common;
using Business.Services.Implementations;
using DataAccess.Entities;
using NUnit.Framework;

namespace Tests.Business
{
    [TestFixture]
    public class SubscriptionServiceTests
    {
        [SetUp]
        public void SetUp()
        {
            _db = new ServicesHelper();

            var profile = new MapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(profile));
            _mapper = new Mapper(configuration);
        }

        [TearDown]
        public void TearDown()
        {
            _db.Dispose();
        }

        private ServicesHelper _db = null!;
        private IMapper _mapper = null!;

        

        private async Task SeedSubscriptions()
        {
            var subscriber = await _db.Context.Users.FindAsync("vasya");
            var target = await _db.Context.Users.FindAsync("petya");
            var t2 = await _db.Context.Users.FindAsync("george");

            foreach (var sub in new[]
            {
                new Subscription
                {
                    Subscriber = subscriber,
                    Target = target,
                    IsNotActive = true,
                },
                new Subscription
                {
                    Subscriber = subscriber,
                    Target = t2,
                },
            })
            {
                await _db.Context.Subscriptions.AddAsync(sub);
            }

            _db.ReloadContext();
        }

        [Test]
        public async Task SubscribeTo()
        {
            // arrange
            await _db.SeedUsers();
            var subscriber = await _db.Context.Users.FindAsync("vasya");
            var target = await _db.Context.Users.FindAsync("petya");

            var service = new SubscriptionService(_db.Context, _mapper,
                new TypedClaimsPrincipal
                {
                    Name = subscriber.Login,
                    Role = Roles.User
                });

            // act
            await service.SubscribeTo(target.Login);

            // assert
            var sub1 = subscriber.Subscriptions.First();
            var sub2 = target.Subscribers.First();
            Assert.AreSame(sub1, sub2);
            Assert.AreEqual(subscriber.Login, sub1.Subscriber!.Login);
            Assert.AreEqual(target.Login, sub1.Target!.Login);
            Assert.AreEqual(false, sub1.IsNotActive);
        }

        [Test]
        public async Task SubscribeTo_SubUnsubSub()
        {
            // arrange
            await _db.SeedUsers();
            var subscriber = await _db.Context.Users.FindAsync("vasya");
            var target = await _db.Context.Users.FindAsync("petya");
            await _db.Context.Subscriptions.AddAsync(new Subscription
            {
                Subscriber = subscriber,
                Target = target,
                IsNotActive = true,
            });
            _db.ReloadContext();

            subscriber = await _db.Context.Users.FindAsync("vasya");
            target = await _db.Context.Users.FindAsync("petya");


            var service = new SubscriptionService(_db.Context, _mapper,
                new TypedClaimsPrincipal
                {
                    Name = subscriber.Login,
                    Role = Roles.User
                });

            // act
            await service.SubscribeTo(target.Login);

            // assert
            var sub1 = subscriber.Subscriptions.First();
            var sub2 = target.Subscribers.First();
            Assert.AreSame(sub1, sub2);
            Assert.AreEqual(subscriber.Login, sub1.Subscriber!.Login);
            Assert.AreEqual(target.Login, sub1.Target!.Login);
            Assert.AreEqual(false, sub1.IsNotActive);
        }

        [Test]
        public async Task UnsubFrom()
        {
            // arrange
            await _db.SeedUsers();
            var subscriber = await _db.Context.Users.FindAsync("vasya");
            var target = await _db.Context.Users.FindAsync("petya");

            await _db.Context.Subscriptions.AddAsync(new Subscription
            {
                Subscriber = subscriber,
                Target = target,
            });
            _db.ReloadContext();

            subscriber = await _db.Context.Users.FindAsync("vasya");
            target = await _db.Context.Users.FindAsync("petya");

            var service = new SubscriptionService(_db.Context, _mapper,
                new TypedClaimsPrincipal
                {
                    Name = subscriber.Login,
                    Role = Roles.User
                });

            // act
            await service.UnsubscribeFrom(target.Login);

            // assert
            var sub1 = subscriber.Subscriptions.First();
            var sub2 = target.Subscribers.First();
            Assert.AreSame(sub1, sub2);
            Assert.AreEqual(subscriber.Login, sub1.Subscriber!.Login);
            Assert.AreEqual(target.Login, sub1.Target!.Login);
            Assert.AreEqual(true, sub1.IsNotActive);
        }

        [Test]
        public async Task GetUserSubscribedTo()
        {
            // arrange
            await _db.SeedUsers();
            await SeedSubscriptions();

            var subscriber = await _db.Context.Users.FindAsync("vasya");

            var service = new SubscriptionService(_db.Context, _mapper,
                new TypedClaimsPrincipal());

            // act
            var subs = (await service.GetUserSubscribedTo(subscriber.Login)).ToArray();

            // assert
            Assert.AreEqual(1, subs.Length);
            Assert.AreEqual(true, subs[0].IsActive);
            Assert.AreEqual(subscriber.Login, subs[0].SubscriberId);
        }

        [Test]
        public async Task IsUserSubscribedTo()
        {
            // arrange
            await _db.SeedUsers();
            await SeedSubscriptions();

            var subscriber = await _db.Context.Users.FindAsync("vasya");
            var t1 = await _db.Context.Users.FindAsync("petya");
            var t2 = await _db.Context.Users.FindAsync("george");
            var t3 = await _db.Context.Users.FindAsync("steve");

            var service = new SubscriptionService(_db.Context, _mapper,
                new TypedClaimsPrincipal
                {
                    Name = subscriber.Login,
                    Role = Roles.User,
                });

            // act
            var subSelf = await service.IsUserSubscribedTo(subscriber.Login);
            var sub2 = await service.IsUserSubscribedTo(t1.Login);
            var sub3 = await service.IsUserSubscribedTo(t2.Login);
            var sub4 = await service.IsUserSubscribedTo(t3.Login);

            // assert
            Assert.AreEqual(false, subSelf);
            Assert.AreEqual(false, sub2);
            Assert.AreEqual(true, sub3);
            Assert.AreEqual(false, sub4);
        }
    }
}