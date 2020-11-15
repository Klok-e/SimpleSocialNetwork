using System;
using System.Threading.Tasks;
using AutoMapper;
using Business;
using Business.Common;
using Business.Models;
using Business.Models.Answers;
using Business.Services;
using Business.Services.Implementations;
using Business.Validation;
using DataAccess;
using DataAccess.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NUnit.Framework;

namespace Tests.Business
{
    [TestFixture]
    public class UserServiceTests
    {
        private SqliteConnection _connection;
        private SocialDbContext _context;
        private IMapper _mapper;
        private IOptions<AppSettings> _settings;

        [SetUp]
        public void SetUp()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();
            var options = new DbContextOptionsBuilder<SocialDbContext>()
                .UseSqlite(_connection)
                .UseLazyLoadingProxies()
                .Options;

            _context = new SocialDbContext(options);
            _context.Database.EnsureCreated();

            var profile = new MapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(profile));
            _mapper = new Mapper(configuration);

            _settings = Options.Create(new AppSettings
                {Secret = "123456789asdddddasdafafwgerghrtyhergkernuitvlgbilerklgergjb"});
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
            var userService = new AuthService(_context, _mapper, _settings);
            var login = "abcde";
            var pass = "12345";

            // act
            var user = await userService.Register(login, pass);

            var _ = await userService.Login(login, pass);

            Func<Task> throws = async () => await userService.Login(login, pass + "a");

            var dbUser = await _context.Users!.FindAsync(user.Login);

            // assert
            Assert.AreEqual(user, _mapper.Map<ApplicationUser, UserModel>(dbUser));

            Assert.ThrowsAsync<ValidationException>(async () => await throws());
        }
    }
}