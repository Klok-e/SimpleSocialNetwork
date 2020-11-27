using System;
using System.Threading.Tasks;
using AutoMapper;
using Business;
using Business.Common;
using Business.Models.Responses;
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
    public class AuthServiceTests
    {
        [SetUp]
        public void SetUp()
        {
            _db = new ServicesHelper();

            var profile = new MapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(profile));
            _mapper = new Mapper(configuration);

            _settings = Options.Create(new AppSettings
                {Secret = "123456789asdddddasdafafwgerghrtyhergkernuitvlgbilerklgergjb"});
        }

        [TearDown]
        public void TearDown()
        {
            _db.Dispose();
        }

        private ServicesHelper _db = null!;
        private IMapper _mapper = null!;
        private IOptions<AppSettings> _settings = null!;

        [Test]
        public async Task Register_Happy()
        {
            // arrange
            var userService = new AuthService(_db.Context, _mapper, _settings);
            var login = "abcde";
            var pass = "12345";

            // act
            var user = await userService.Register(login, pass);
            var dbUser = await _db.Context.Users.FindAsync(user.Login);
            var dbUser1 = await _db.Context.Users.FindAsync(login);

            // assert
            Assert.AreEqual(dbUser, dbUser1);
            Assert.AreEqual(user, _mapper.Map<ApplicationUser, UserModel>(dbUser));
        }

        [Test]
        public async Task Register_UserExistsThrowsValidationException()
        {
            // arrange
            var userService = new AuthService(_db.Context, _mapper, _settings);
            var login = "abcde";
            var pass = "12345";
            await userService.Register(login, pass);

            // act
            Func<Task> throws = async () => await userService.Register(login, pass);

            // assert
            Assert.ThrowsAsync<ValidationException>(async () => await throws());
        }

        [Test]
        public async Task Login_Happy()
        {
            // arrange
            var userService = new AuthService(_db.Context, _mapper, _settings);
            var login = "abcde";
            var pass = "12345";
            await userService.Register(login, pass);

            // act
            var loggedInUser = await userService.Login(login, pass);

            // assert
            Assert.AreEqual(login, loggedInUser.Login);
        }

        [Test]
        public async Task Login_CantLogWrongPassword()
        {
            // arrange
            var userService = new AuthService(_db.Context, _mapper, _settings);
            var login = "abcde";
            var pass = "12345";
            await userService.Register(login, pass);

            // act
            Func<Task> throws = async () => await userService.Login(login, pass + "a");

            // assert
            Assert.ThrowsAsync<ValidationException>(async () => await throws());
        }
    }
}