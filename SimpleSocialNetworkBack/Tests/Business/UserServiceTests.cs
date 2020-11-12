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
                .UseLazyLoadingProxies()
                .Options;

            _context = new SocialDbContext(options, null);
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


            // assert
        }
    }
}