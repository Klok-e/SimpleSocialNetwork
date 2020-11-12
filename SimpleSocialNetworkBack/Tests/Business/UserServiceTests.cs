using System.Threading.Tasks;
using AutoMapper;
using Business;
using Business.Services;
using DataAccess;
using NUnit.Framework;

namespace Tests.Business
{
    [TestFixture]
    public class UserServiceTests
    {
        private SqliteInMemory _db;
        private SocialDbContext _context;
        private IMapper _mapper;

        [SetUp]
        public void SetUp()
        {
            _db = new SqliteInMemory();
            _context = _db.InMemoryDatabase();

            var profile = new MapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(profile));
            _mapper = new Mapper(configuration);
        }

        [TearDown]
        public void TearDown()
        {
            _db.Dispose();
        }

        [Test]
        public async Task UserService_Register_Happy()
        {
            // arrange
            var userService = new UserService(_context, _mapper);
            var login = "abcde";
            var pass = "12345";

            // act
            var user = await userService.Register(login, pass);

            var dbUser = await _context.Users!.FindAsync(user.Login);

            // assert
            Assert.AreEqual(user, dbUser);
        }
    }
}