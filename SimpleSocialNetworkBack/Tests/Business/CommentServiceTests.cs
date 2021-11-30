using System;
using System.Linq;
using System.Threading.Tasks;
using Business.Common;
using Business.Models.Requests;
using Business.Services.Implementations;
using Business.Validation;
using DataAccess.Entities;
using NUnit.Framework;

namespace Tests.Business
{
    [TestFixture]
    public class CommentServiceTests
    {
        private ServicesHelper _db = null!;

        [SetUp]
        public void SetUp()
        {
            _db = new ServicesHelper();
        }

        [TearDown]
        public void TearDown()
        {
            _db.Dispose();
        }

        [Test]
        public async Task CreateComment_Happy()
        {
            // arrange
            await SeedData();
            var u = await _db.Context.Users.FindAsync("vasya");
            const string expectedContent = "foobar";
            await _db.Context.SaveChangesAsync();
            _db.ReloadContext();

            var commentService = new CommentService(_db.Context,
                new TypedClaimsPrincipal { Name = u.Login, Role = Roles.User }, true);

            // act
            await commentService.CreateComment(new CreateCommentModel { OpId = 1, Content = expectedContent });

            // assert
            var msg = (await _db.Context.OpMessages.FindAsync(1)).Messages.Last();
            Assert.AreEqual(expectedContent, msg.Content);
            Assert.AreEqual(1, msg.OpId);
        }

        [Test]
        public async Task CreateComment_SecondComment()
        {
            // arrange
            const string expectedContent = "foobar";
            await SeedData();
            var u = await _db.Context.Users.FindAsync("vasya");
            u.Posts.First()
             .Messages.Add(new Message { MessageId = 1, Content = "123456", Poster = u });

            await _db.Context.SaveChangesAsync();
            _db.ReloadContext();

            var commentService = new CommentService(_db.Context,
                new TypedClaimsPrincipal { Name = u.Login, Role = Roles.User }, true);

            // act
            await commentService.CreateComment(new CreateCommentModel { OpId = 1, Content = expectedContent });

            // assert
            var msg = (await _db.Context.OpMessages.FindAsync(1)).Messages.Last();
            Assert.AreEqual(expectedContent, msg.Content);
            Assert.AreEqual(1, msg.OpId);
        }

        [Test]
        public async Task VoteComment_Happy()
        {
            // arrange
            await SeedData();
            var u = await _db.Context.Users.FindAsync("vasya");
            u.Posts.First()
             .Messages.Add(new Message { MessageId = 1, Content = "123456", Poster = u });

            await _db.Context.SaveChangesAsync();
            _db.ReloadContext();

            var commentService = new CommentService(_db.Context,
                new TypedClaimsPrincipal { Name = u.Login, Role = Roles.User });

            // act
            await commentService.VoteComment(new VoteComment
            {
                CommentId = new CommentKeyModel { MessageId = 1, OpId = 1 }, VoteType = VoteType.Up
            });
            await commentService.VoteComment(new VoteComment
            {
                CommentId = new CommentKeyModel { MessageId = 1, OpId = 1 }, VoteType = VoteType.Down
            });
            await commentService.VoteComment(new VoteComment
            {
                CommentId = new CommentKeyModel { MessageId = 1, OpId = 1 }, VoteType = VoteType.Down
            });

            // assert
            var comment = (await _db.Context.OpMessages.FindAsync(1)).Messages.Last();
            Assert.AreEqual(-1, comment.Points);
        }

        [Test]
        public async Task DeleteComment_Happy()
        {
            // arrange
            await SeedData();
            var u = await _db.Context.Users.FindAsync("vasya");
            u.Posts.First()
             .Messages.Add(new Message { MessageId = 1, Content = "123456", Poster = u });

            await _db.Context.SaveChangesAsync();
            _db.ReloadContext();

            var commentService = new CommentService(_db.Context,
                new TypedClaimsPrincipal { Name = u.Login, Role = Roles.User });
            var commentBeforeDel = (await _db.Context.OpMessages.FindAsync(1)).Messages.Last()
                                                                              .IsDeleted;

            // act
            await commentService.DeleteCommentSoft(new CommentKeyModel { MessageId = 1, OpId = 1 });

            // assert
            Assert.AreEqual(false, commentBeforeDel);
            var commentDel = (await _db.Context.OpMessages.FindAsync(1)).Messages.Last()
                                                                        .IsDeleted;
            Assert.AreEqual(true, commentDel);
        }

        [Test]
        public async Task DeleteComment_NotSelfCommentThrowsForbidden()
        {
            // arrange
            await SeedData();
            var u = await _db.Context.Users.FindAsync("vasya");
            u.Posts.First()
             .Messages.Add(new Message { MessageId = 1, Content = "123456", Poster = u });

            await _db.Context.SaveChangesAsync();
            _db.ReloadContext();

            var commentService = new CommentService(_db.Context,
                new TypedClaimsPrincipal { Name = "george", Role = Roles.User });

            // act
            Func<Task> throws = async () =>
                await commentService.DeleteCommentSoft(new CommentKeyModel { MessageId = 1, OpId = 1 });

            // assert
            Assert.ThrowsAsync<ForbiddenException>(async () => await throws());
        }

        [Test]
        public async Task DeleteComment_WithoutPosterNotAdminThrowsForbidden()
        {
            // arrange
            await SeedData();
            var u = await _db.Context.Users.FindAsync("vasya");
            u.Posts.First()
             .Messages.Add(new Message { MessageId = 1, Content = "123456" });

            await _db.Context.SaveChangesAsync();
            _db.ReloadContext();

            var commentService = new CommentService(_db.Context,
                new TypedClaimsPrincipal { Name = "george", Role = Roles.User });

            // act
            async Task Throws()
            {
                await commentService.DeleteCommentSoft(new CommentKeyModel { MessageId = 1, OpId = 1 });
            }

            // assert
            Assert.ThrowsAsync<ForbiddenException>(async () => await Throws());
        }

        private async Task SeedData()
        {
            await _db.Context.Users.AddAsync(new ApplicationUser { Login = "petya", IsAdmin = true });

            const string login = "vasya";
            var u = await _db.Context.Users.AddAsync(new ApplicationUser { Login = login });
            await _db.Context.OpMessages.AddAsync(new OpMessage
            {
                Title = "bar", Content = "foo", Id = 1, Poster = u.Entity
            });
            await _db.Context.SaveChangesAsync();
            _db.ReloadContext();
        }
    }
}
