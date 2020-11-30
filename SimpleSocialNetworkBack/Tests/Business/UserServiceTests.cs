using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business;
using Business.Common;
using Business.Models.Requests;
using Business.Services.Implementations;
using Business.Validation;
using DataAccess.Entities;
using NUnit.Framework;

namespace Tests.Business
{
    [TestFixture]
    public class UserServiceTests
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

        private async Task SeedBan()
        {
            var admin = await _db.Context.Users.FindAsync("petya");
            var u1 = await _db.Context.Users.FindAsync("vasya");
            //var u2 = await _db.Context.Users.FindAsync("george");

            foreach (var sub in new[]
            {
                new TagBan
                {
                    Moderator = admin,
                    User = u1,
                    Tag = new Tag {Name = ""},
                    ExpirationDate = new DateTime(3000, 1, 1),
                    BanIssuedDate = DateTime.UtcNow
                }
            })
            {
                await _db.Context.TagBans.AddAsync(sub);
            }

            _db.ReloadContext();
        }

        [Test]
        public async Task GetUser()
        {
            // arrange
            await _db.SeedUsers();

            var user = await _db.Context.Users.FindAsync("vasya");

            var service = new UserService(_db.Context, _mapper,
                new TypedClaimsPrincipal
                {
                    Role = Roles.Admin
                });

            // act
            var u = await service.GetUser(user.Login);

            // assert
            Assert.AreEqual(user.Login, u.Login);
            Assert.AreEqual(false, u.IsDeleted);
        }

        [Test]
        public async Task GetUser_NotAdminThrowsForbidden()
        {
            // arrange
            await _db.SeedUsers();

            var user = await _db.Context.Users.FindAsync("vasya");

            var service = new UserService(_db.Context, _mapper,
                new TypedClaimsPrincipal
                {
                    Role = Roles.User
                });

            // act
            async Task Throws() => await service.GetUser(user.Login);

            // assert
            Assert.ThrowsAsync<ForbiddenException>(Throws);
        }

        [Test]
        public async Task GetUserLimited()
        {
            // arrange
            await _db.SeedUsers();

            var user = await _db.Context.Users.FindAsync("vasya");

            var service = new UserService(_db.Context, _mapper,
                new TypedClaimsPrincipal
                {
                    Role = Roles.User
                });

            // act
            var u = await service.GetUserLimited(user.Login);

            // assert
            Assert.AreEqual(user.Login, u.Login);
            Assert.AreEqual(false, u.IsDeleted);
        }

        [Test]
        public async Task UserExists()
        {
            // arrange
            await _db.SeedUsers();

            var user = await _db.Context.Users.FindAsync("vasya");

            var service = new UserService(_db.Context, _mapper, new TypedClaimsPrincipal());

            // act
            var exists = await service.UserExists(user.Login);

            // assert
            Assert.AreEqual(true, exists);
        }

        [Test]
        public async Task UserExists_DeletedUserExistsToo()
        {
            // arrange
            await _db.SeedUsers();

            var user = await _db.Context.Users.FindAsync("steve");

            var service = new UserService(_db.Context, _mapper, new TypedClaimsPrincipal());

            // act
            var exists = await service.UserExists(user.Login);

            // assert
            Assert.AreEqual(true, exists);
        }

        [Test]
        public async Task ChangeUserInfo()
        {
            // arrange
            await _db.SeedUsers();

            var user = await _db.Context.Users.FindAsync("vasya");

            var service = new UserService(_db.Context, _mapper, new TypedClaimsPrincipal
            {
                Name = user.Login,
                Role = Roles.User,
            });
            var info = new ChangeUserInfo
            {
                About = "foobarbaz foobarbaz foobarbaz foobarbaz",
                DateBirth = new DateTime(50, 4, 23),
            };

            // act
            await service.ChangeUserInfo(info);

            // assert
            Assert.AreEqual(info.About, user.About);
            Assert.AreEqual(info.DateBirth, user.DateBirth);
        }

        [Test]
        public async Task SearchUsers_EmptyReturnsAllUsers()
        {
            // arrange
            await _db.SeedUsers();

            var service = new UserService(_db.Context, _mapper, new TypedClaimsPrincipal());
            var expectedUserList = new[]
            {
                "george", "petya", "vasya",
            };

            // act
            var users = (await service.SearchUsers(new SearchUsersModel()))
                .Select(x => x.Login)
                .ToArray();

            // assert
            Assert.AreEqual(expectedUserList, users);
        }

        [Test]
        public async Task SearchUsers_About()
        {
            // arrange
            await _db.SeedUsers();

            var service = new UserService(_db.Context, _mapper, new TypedClaimsPrincipal());
            var expectedUserList = new[]
            {
                "vasya",
            };

            // act
            var users = (await service.SearchUsers(new SearchUsersModel
                {
                    AboutPattern = "sff"
                }))
                .Select(x => x.Login)
                .ToArray();

            // assert
            Assert.AreEqual(expectedUserList, users);
        }

        [Test]
        public async Task SearchUsers_Name()
        {
            // arrange
            await _db.SeedUsers();

            var service = new UserService(_db.Context, _mapper, new TypedClaimsPrincipal());
            var expectedUserList = new[]
            {
                "petya", "vasya"
            };

            // act
            var users = (await service.SearchUsers(new SearchUsersModel
                {
                    NamePattern = "ya"
                }))
                .Select(x => x.Login)
                .ToArray();

            // assert
            Assert.AreEqual(expectedUserList, users);
        }

        [Test]
        public async Task BanUser()
        {
            // arrange
            await _db.SeedUsers();

            var user = await _db.Context.Users.FindAsync("petya");

            var service = new UserService(_db.Context, _mapper, new TypedClaimsPrincipal
            {
                Name = user.Login,
                Role = Roles.Admin,
            });

            var banTarget = await _db.Context.Users.FindAsync("vasya");
            var banExp = new DateTime(3000, 1, 1);

            // act
            await service.BanUser(new BanUserModel
            {
                Login = banTarget.Login,
                ExpirationDate = banExp,
            });

            // assert
            Assert.AreEqual(banExp, banTarget.BansReceived.First().ExpirationDate);
            Assert.AreEqual(false, banTarget.BansReceived.First().IsDeleted);
        }

        [Test]
        public async Task LiftBanFromUser()
        {
            // arrange
            await _db.SeedUsers();
            await SeedBan();

            var admin = await _db.Context.Users.FindAsync("petya");

            var service = new UserService(_db.Context, _mapper, new TypedClaimsPrincipal
            {
                Name = admin.Login,
                Role = Roles.Admin,
            });

            var banTarget = await _db.Context.Users.FindAsync("vasya");

            // act
            await service.LiftBanFromUser(banTarget.Login);

            // assert
            Assert.AreEqual(true, banTarget.BansReceived.First().IsDeleted);
        }

        [Test]
        public async Task UserDeleted()
        {
            // arrange
            await _db.SeedUsers();

            var service = new UserService(_db.Context, _mapper, new TypedClaimsPrincipal());

            // act
            var deleted = await service.UserDeleted("vasya");
            var deleted2 = await service.UserDeleted("steve");

            // assert
            Assert.AreEqual(false, deleted);
            Assert.AreEqual(true, deleted2);
        }

        [Test]
        public async Task UserDeleted_NonexistentThrowsValidation()
        {
            // arrange
            await _db.SeedUsers();

            var service = new UserService(_db.Context, _mapper, new TypedClaimsPrincipal());

            // act
            async Task Throws() => await service.UserDeleted("vasya123456");

            // assert
            Assert.ThrowsAsync<ValidationException>(Throws);
        }

        [Test]
        public async Task ElevateUser()
        {
            // arrange
            await _db.SeedUsers();

            var admin = await _db.Context.Users.FindAsync("petya");

            var service = new UserService(_db.Context, _mapper, new TypedClaimsPrincipal
            {
                Name = admin.Login,
                Role = Roles.Admin,
            });

            var target = await _db.Context.Users.FindAsync("vasya");

            // act
            await service.ElevateUser(target.Login);

            // assert
            Assert.AreEqual(true, target.IsAdmin);
        }

        [Test]
        public async Task ElevateUser_NotAdminThrowsForbidden()
        {
            // arrange
            await _db.SeedUsers();
            await SeedBan();

            var admin = await _db.Context.Users.FindAsync("petya");

            var service = new UserService(_db.Context, _mapper, new TypedClaimsPrincipal
            {
                Name = admin.Login,
                Role = Roles.User,
            });

            var target = await _db.Context.Users.FindAsync("vasya");

            // act
            async Task Throws() => await service.ElevateUser(target.Login);

            // assert
            Assert.ThrowsAsync<ForbiddenException>(Throws);
        }

        [Test]
        public async Task DeleteUserSoft()
        {
            // arrange
            await _db.SeedUsers();

            var service = new UserService(_db.Context, _mapper, new TypedClaimsPrincipal
            {
                Name = "petya",
                Role = Roles.Admin,
            });

            var iseDelBefore = (await _db.Context.Users.FindAsync("vasya")).IsDeleted;

            // act
            await service.DeleteUserSoft("vasya");

            // assert
            Assert.AreEqual(false, iseDelBefore);
            var iseDel = (await _db.Context.Users.FindAsync("vasya")).IsDeleted;
            Assert.AreEqual(true, iseDel);
        }

        [Test]
        public async Task UserBanned()
        {
            // arrange
            await _db.SeedUsers();
            await SeedBan();

            var admin = await _db.Context.Users.FindAsync("petya");

            var service = new UserService(_db.Context, _mapper, new TypedClaimsPrincipal
            {
                Name = admin.Login,
                Role = Roles.Admin,
            });

            // act
            var banned = await service.UserBanned("vasya");
            var banned2 = await service.UserBanned("petya");

            // assert
            Assert.AreEqual(true, banned);
            Assert.AreEqual(false, banned2);
        }

        [Test]
        public async Task UserBanned_NotAdminThrowsForbidden()
        {
            // arrange
            await _db.SeedUsers();

            var u = await _db.Context.Users.FindAsync("vasya");

            var service = new UserService(_db.Context, _mapper, new TypedClaimsPrincipal
            {
                Name = u.Login,
                Role = Roles.User,
            });

            // act
            var selfBanned = await service.UserBanned("vasya");
            async Task Throws() => await service.UserBanned("petya");

            // assert
            Assert.AreEqual(false, selfBanned);
            Assert.ThrowsAsync<ForbiddenException>(Throws);
        }
    }
}