using System;
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

            await _context.Messages.AddAsync(new Message
            {
                OpMessage = op,
                Poster = userEnt,
                Content = comment.Content,
                SendDate = DateTime.UtcNow
            });
            await _context.SaveChangesAsync();
        }

        public async Task VoteComment(VoteComment vote)
        {
            var message = await _context.Messages.FindAsync(vote.OpId, vote.MessageId);
            ExceptionHelper.CheckEntitySoft(message, "comment");

            message.Points += vote.VoteType switch
            {
                VoteType.Up => 1,
                VoteType.Down => -1,
                _ => throw new ValidationException("Unknown VoteType value")
            };
            await _context.SaveChangesAsync();
        }
    }
}