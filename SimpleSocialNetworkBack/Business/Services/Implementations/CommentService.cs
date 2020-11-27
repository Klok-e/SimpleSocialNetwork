using System;
using System.Linq;
using System.Threading.Tasks;
using Business.Common;
using Business.Models.Requests;
using Business.Validation;
using DataAccess;
using DataAccess.Entities;

namespace Business.Services.Implementations
{
    public class CommentService : ICommentService
    {
        private readonly SocialDbContext _context;
        private readonly TypedClaimsPrincipal _principal;

        public CommentService(
            SocialDbContext context,
            TypedClaimsPrincipal principal)
        {
            _context = context;
            _principal = principal;
        }

        public async Task CreateComment(CreateCommentModel comment)
        {
            var userEnt = await _context.Users.FindAsync(_principal.Name);
            ExceptionHelper.CheckSelfSoft(userEnt, "user commenter");

            var op = await _context.OpMessages.FindAsync(comment.OpId);
            ExceptionHelper.CheckEntitySoft(op, "post");

            // do the unthinkable
            // because sqlite doesn't support composite autoincrement keys (sql server does though)
            // and it's impossible to test without manual msgId specification
            var msgId = op.Messages.Select(x => x.MessageId).DefaultIfEmpty(0).Max() + 1;

            await _context.Messages.AddAsync(new Message
            {
                OpMessage = op,
                MessageId = msgId,
                Poster = userEnt,
                Content = comment.Content,
                SendDate = DateTime.UtcNow,
            });
            await _context.SaveChangesAsync();
        }

        public async Task VoteComment(VoteComment vote)
        {
            var message = await _context.Messages.FindAsync(vote.CommentId.OpId, vote.CommentId.MessageId);
            ExceptionHelper.CheckEntitySoft(message, "comment");

            message.Points += vote.VoteType switch
            {
                VoteType.Up => 1,
                VoteType.Down => -1,
                _ => throw new ValidationException("Unknown VoteType value")
            };
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCommentSoft(CommentKeyModel commentId)
        {
            var message = await _context.Messages.FindAsync(commentId.OpId, commentId.MessageId);
            ExceptionHelper.CheckEntitySoft(message, "comment");
            if (_principal.Role != Roles.Admin
                && message.Poster != null
                && _principal.Name != message.Poster.Login)
                throw new ForbiddenException("Can't delete post if its isn't yours or you're not an admin");
            if (_principal.Role != Roles.Admin && message.Poster == null)
                throw new ForbiddenException("Only admin can do this");

            message.IsDeleted = true;

            await _context.SaveChangesAsync();
        }
    }
}