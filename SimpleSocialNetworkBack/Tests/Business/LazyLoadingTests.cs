using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Tests.Business
{
    [TestFixture]
    public class LazyLoadingTests
    {
        [SetUp]
        public void SetUp()
        {
            _db = new ServicesHelper();

            var profile = new MapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(profile));
        }

        [TearDown]
        public void TearDown()
        {
            _db.Dispose();
        }

        private ServicesHelper _db = null!;

        [Test]
        public async Task CreatesProxy()
        {
            var proxy = _db.Context.Users.CreateProxy();
            proxy.Login = "123456";
            await _db.Context.Users.AddAsync(proxy);
            var msgs = proxy.Messages.ToArray();
            await _db.Context.SaveChangesAsync();

            Assert.AreEqual(new Message[0], msgs);
        }

        [Test]
        public async Task FindProxy()
        {
            await _db.Context.Users.AddAsync(new ApplicationUser
            {
                Login = "abcde"
            });
            await _db.Context.SaveChangesAsync();
            _db.ReloadContext();

            var user = await _db.Context.Users.FindAsync("abcde");
            var msgs = user.Messages.ToArray();
            await _db.Context.OpMessages.AddAsync(new OpMessage
            {
                Title = "1234",
                Content = "qwerty",
                Poster = user,
            });
            await _db.Context.SaveChangesAsync();

            Assert.AreEqual(new Message[0], msgs);
        }
    }
}