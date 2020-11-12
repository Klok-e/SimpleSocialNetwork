using System.Threading.Tasks;
using AutoMapper;
using Business;
using Business.Models;
using Business.Services;
using DataAccess;
using DataAccess.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Tests.Business
{
    [TestFixture]
    public class UserServiceTests
    {
        private SqliteConnection _connection;
        private SocialDbContext _context;
        private IMapper _mapper;

        [SetUp]
        public void SetUp()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();
            var options = new DbContextOptionsBuilder<SocialDbContext>()
                .UseSqlite(_connection)
                .Options;

            _context = new SocialDbContext(options);
            _context.Database.EnsureCreated();

            var profile = new MapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(profile));
            _mapper = new Mapper(configuration);
        }

        [TearDown]
        public void TearDown()
        {
            _connection.Close();
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

            var dbUser = _mapper.Map<User, UserModel>(await _context.Users!.FindAsync(user.Login));

            // assert
            Assert.IsTrue(UserModel.UserModelComparer.Equals(user, dbUser));
        }
    }
}