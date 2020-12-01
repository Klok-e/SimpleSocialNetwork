using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business;
using Business.Common;
using Business.Models.Requests;
using Business.Models.Responses;
using Business.Services.Implementations;
using Business.Validation;
using DataAccess.Entities;
using NUnit.Framework;

namespace Tests.Business
{
    [TestFixture]
    public class OpMessageServiceTests
    {
        [SetUp]
        public void SetUp()
        {
            _db = new ServicesHelper();

            var profile = new MapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(profile));
            _mapper = new Mapper(configuration);
            _commentId = 0;
        }

        [TearDown]
        public void TearDown()
        {
            _db.Dispose();
        }

        private ServicesHelper _db = null!;
        private IMapper _mapper = null!;

        private async Task SeedUsers()
        {
            await _db.Context.Users.AddAsync(new ApplicationUser
            {
                Login = "petya",
                IsAdmin = true,
            });

            const string login = "vasya";
            var u = await _db.Context.Users.AddAsync(new ApplicationUser
            {
                Login = login,
            });
            await _db.Context.SaveChangesAsync();
            _db.ReloadContext();
        }

        private async Task SeedPost(ApplicationUser? poster = null)
        {
            await _db.Context.OpMessages.AddAsync(new OpMessage
            {
                Content = "1234",
                Title = "foo",
                Poster = poster,
            });

            await _db.Context.SaveChangesAsync();
            _db.ReloadContext();
        }

        private async Task SeedSeveralPosts(ApplicationUser? poster = null, Func<OpMessage, Task>? postMod = null)
        {
            foreach (var post in new[]
            {
                new OpMessage
                {
                    Title = "foo",
                    Content = "bar",
                    SendDate = new DateTime(2020, 11, 29),
                    Poster = poster
                },
                new OpMessage
                {
                    Title = "fook",
                    Content = "baz",
                    SendDate = new DateTime(2020, 11, 25),
                    Poster = poster
                },
                new OpMessage
                {
                    Title = "foobard",
                    Content = "contentd",
                    SendDate = new DateTime(2020, 12, 2),
                    IsDeleted = true,
                    Poster = poster
                },
                new OpMessage
                {
                    Title = "foobar",
                    Content = "content",
                    SendDate = new DateTime(2020, 12, 1),
                    Poster = poster
                },
            })
            {
                if (postMod != null)
                    await postMod.Invoke(post);
                await _db.Context.OpMessages.AddAsync(post);
            }

            await _db.Context.SaveChangesAsync();
            _db.ReloadContext();
        }

        private int _commentId;

        private async Task SeedComments(OpMessage post, ApplicationUser? poster = null)
        {
            foreach (var comment in new[]
            {
                new Message
                {
                    Content = "bar",
                    SendDate = new DateTime(2020, 11, 29),
                    Poster = poster,
                    OpMessage = post,
                    MessageId = ++_commentId,
                },
                new Message
                {
                    Content = "baz",
                    SendDate = new DateTime(2020, 11, 25),
                    Poster = poster,
                    OpMessage = post,
                    MessageId = ++_commentId,
                },
                new Message
                {
                    Content = "contentd",
                    SendDate = new DateTime(2020, 12, 2),
                    IsDeleted = true,
                    Poster = poster,
                    OpMessage = post,
                    MessageId = ++_commentId,
                },
                new Message
                {
                    Content = "content",
                    SendDate = new DateTime(2020, 12, 1),
                    Poster = poster,
                    OpMessage = post,
                    MessageId = ++_commentId,
                },
            })
            {
                await _db.Context.Messages.AddAsync(comment);
            }
        }

        [Test]
        public async Task CreatePost_Happy()
        {
            // arrange
            await SeedUsers();
            var u = await _db.Context.Users.FindAsync("vasya");
            const string expectedTitle = "foobarbaz";
            const string expectedContent = "foobar";

            var service = new OpMessageService(_db.Context, _mapper,
                new TypedClaimsPrincipal
                {
                    Name = u.Login,
                    Role = Roles.User
                });

            // act
            var id = await service.MakeAPost(new CreateOpMessageModel
            {
                Title = expectedTitle,
                Content = expectedContent,
            });

            // assert
            var msg = await _db.Context.OpMessages.FindAsync(id);
            Assert.AreEqual(expectedTitle, msg.Title);
            Assert.AreEqual(expectedContent, msg.Content);
            Assert.AreEqual(u.Login, msg.Poster!.Login);
        }

        // [Test]
        // public async Task VotePost_Happy()
        // {
        //     // arrange
        //     await SeedUsers();
        //     var u = await _db.Context.Users.FindAsync("vasya");
        //     await SeedPost(u);
        //
        //     var service = new OpMessageService(_db.Context, _mapper,
        //         new TypedClaimsPrincipal
        //         {
        //             Name = u.Login,
        //             Role = Roles.User
        //         });
        //
        //     // act
        //     await service.VotePost(new VotePost
        //     {
        //         PostId = 1,
        //         VoteType = VoteType.Up
        //     });
        //     await service.VotePost(new VotePost
        //     {
        //         PostId = 1,
        //         VoteType = VoteType.Down
        //     });
        //     await service.VotePost(new VotePost
        //     {
        //         PostId = 1,
        //         VoteType = VoteType.Down
        //     });
        //
        //     // assert
        //     var post = await _db.Context.OpMessages.FindAsync(1);
        //     Assert.AreEqual(-1, post.Points);
        // }

        [Test]
        public async Task DeletePost_Happy()
        {
            // arrange
            await SeedUsers();
            var u = await _db.Context.Users.FindAsync("vasya");
            await SeedPost(u);

            await _db.Context.SaveChangesAsync();
            _db.ReloadContext();

            var service = new OpMessageService(_db.Context, _mapper,
                new TypedClaimsPrincipal
                {
                    Name = u.Login,
                    Role = Roles.User
                });
            var beforeDel = (await _db.Context.OpMessages.FindAsync(1)).IsDeleted;

            // act
            await service.DeletePostSoft(1);

            // assert
            Assert.AreEqual(false, beforeDel);
            var commentDel = (await _db.Context.OpMessages.FindAsync(1)).IsDeleted;
            Assert.AreEqual(true, commentDel);
        }

        [Test]
        public async Task DeletePost_NotSelfPostThrowsForbidden()
        {
            // arrange
            await SeedUsers();
            var u = await _db.Context.Users.FindAsync("vasya");
            await SeedPost(u);

            await _db.Context.SaveChangesAsync();
            _db.ReloadContext();

            var service = new OpMessageService(_db.Context, _mapper,
                new TypedClaimsPrincipal
                {
                    Name = "george",
                    Role = Roles.User
                });

            // act
            async Task Throws() => await service.DeletePostSoft(1);

            // assert
            Assert.ThrowsAsync<ForbiddenException>(async () => await Throws());
        }

        [Test]
        public async Task DeletePost_WithoutPosterNotAdminThrowsForbidden()
        {
            // arrange
            await SeedPost();

            await _db.Context.SaveChangesAsync();
            _db.ReloadContext();

            var service = new OpMessageService(_db.Context, _mapper,
                new TypedClaimsPrincipal
                {
                    Name = "george",
                    Role = Roles.User
                });

            // act
            async Task Throws() =>
                await service.DeletePostSoft(1);

            // assert
            Assert.ThrowsAsync<ForbiddenException>(async () => await Throws());
        }

        [Test]
        public async Task GetAll_SortedByDateExcludeDeleted()
        {
            // arrange
            await SeedSeveralPosts();

            var service = new OpMessageService(_db.Context, _mapper, new TypedClaimsPrincipal());

            // act
            var posts = (await service.GetAll(0)).ToArray();

            // assert
            Assert.AreEqual(3, posts.Length);
            Assert.AreEqual("foobar", posts[0].Title);
            Assert.AreEqual("foo", posts[1].Title);
            Assert.AreEqual("fook", posts[2].Title);
        }

        [Test]
        public async Task PostsFromUser_SortedByDateExcludeDeleted()
        {
            // arrange
            await SeedUsers();
            var u = await _db.Context.Users.FindAsync("vasya");
            await SeedSeveralPosts(u);
            await SeedPost();

            var service = new OpMessageService(_db.Context, _mapper, new TypedClaimsPrincipal());

            // act
            var posts = (await service.PostsFromUser(u.Login)).ToArray();

            // assert
            Assert.AreEqual(3, posts.Length);
            Assert.AreEqual("foobar", posts[0].Title);
            Assert.AreEqual(u.Login, posts[0].PosterId);
            Assert.AreEqual("foo", posts[1].Title);
            Assert.AreEqual(u.Login, posts[1].PosterId);
            Assert.AreEqual("fook", posts[2].Title);
            Assert.AreEqual(u.Login, posts[2].PosterId);
        }

        [Test]
        public async Task GetById()
        {
            // arrange
            await SeedUsers();
            var u = await _db.Context.Users.FindAsync("vasya");
            await SeedSeveralPosts(u);
            await SeedPost();

            var service = new OpMessageService(_db.Context, _mapper, new TypedClaimsPrincipal());

            // act
            var post = await service.GetById(2);

            // assert
            Assert.AreEqual(2, post.Id);
        }

        [Test]
        public async Task GetById_NonexistentThrowsValidationError()
        {
            // arrange
            await SeedUsers();
            var u = await _db.Context.Users.FindAsync("vasya");
            await SeedSeveralPosts(u);
            await SeedPost();

            var service = new OpMessageService(_db.Context, _mapper, new TypedClaimsPrincipal());

            // act
            async Task<OpMessageModel> Throws() => await service.GetById(20);

            // assert
            Assert.ThrowsAsync<ValidationException>(Throws);
        }

        [Test]
        public async Task GetById_DeletedThrowsValidationError()
        {
            // arrange
            await SeedUsers();
            var u = await _db.Context.Users.FindAsync("vasya");
            await SeedSeveralPosts(u);
            await SeedPost();

            var service = new OpMessageService(_db.Context, _mapper, new TypedClaimsPrincipal());

            // act
            async Task<OpMessageModel> Throws() => await service.GetById(3);

            // assert
            Assert.ThrowsAsync<ValidationException>(Throws);
        }

        [Test]
        public async Task GetComments_SortedByDate()
        {
            // arrange
            await SeedSeveralPosts(null, async post => { await SeedComments(post); });
            await SeedPost();

            var service = new OpMessageService(_db.Context, _mapper, new TypedClaimsPrincipal());

            // act
            var comments = (await service.GetComments(4, 0)).ToArray();

            // assert
            Assert.AreEqual(3, comments.Length);
            Assert.AreEqual("content", comments[0].Content);
            Assert.AreEqual("bar", comments[1].Content);
            Assert.AreEqual("baz", comments[2].Content);
        }

        [Test]
        public async Task PostExists()
        {
            // arrange
            await SeedSeveralPosts();

            var service = new OpMessageService(_db.Context, _mapper, new TypedClaimsPrincipal());

            // act
            var deletedExists = await service.PostExists(3);
            var exists = await service.PostExists(4);
            var nonexistentExists = await service.PostExists(-3);

            // assert
            Assert.AreEqual(false, deletedExists);
            Assert.AreEqual(true, exists);
            Assert.AreEqual(false, nonexistentExists);
        }
    }
}