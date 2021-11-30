using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Common;
using Business.Models.Requests;
using Business.Models.Responses;
using Business.Validation;
using DataAccess;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Business.Services.Implementations
{
    public class OpMessageService : IOpMessageService
    {
        private readonly SocialDbContext _context;
        private readonly IMapper _mapper;
        private readonly TypedClaimsPrincipal _principal;

        public OpMessageService(SocialDbContext context, IMapper mapper, TypedClaimsPrincipal principal)
        {
            _context = context;
            _mapper = mapper;
            _principal = principal;
        }

        #region IOpMessageService Members

        public Task<IEnumerable<OpMessageModel>> GetAll(int page)
        {
            return Task.FromResult(_context.OpMessages
                                           // include to prevent "There is already an open DataReader associated with this Connection which must be closed first."
                                           // error by preloading poster
                                           .Include(x => x.Poster)
                                           .Include(x => x.Votes)
                                           .Where(x => !x.IsDeleted)
                                           .OrderByDescending(x => x.SendDate)
                                           .Select(x => _mapper.Map<OpMessage, OpMessageModel>(x))
                                           .Skip(Constants.PageSize * page)
                                           .Take(Constants.PageSize)
                                           .AsEnumerable());
        }

        public async Task<IEnumerable<OpMessageModel>> PostsFromUser(string login)
        {
            var user = await _context.Users.FindAsync(login);
            ExceptionHelper.CheckEntitySoft(user, "user");

            return user.Posts.Where(x => !x.IsDeleted)
                       .OrderByDescending(x => x.SendDate)
                       .Select(x => _mapper.Map<OpMessage, OpMessageModel>(x));
        }

        public async Task DeletePostSoft(int postId)
        {
            var opMessage = await _context.OpMessages.FindAsync(postId);
            ExceptionHelper.CheckEntitySoft(opMessage, "opMessage");
            if (_principal.Role != Roles.Admin && opMessage.Poster != null && _principal.Name != opMessage.Poster.Login)
                throw new ForbiddenException("Can't delete post if its isn't yours or you're not an admin");
            if (opMessage.Poster == null && _principal.Role != Roles.Admin)
                throw new ForbiddenException("Only admin can delete posts without posters");

            opMessage.IsDeleted = true;

            await _context.SaveChangesAsync();
        }

        public async Task<int> MakeAPost(CreateOpMessageModel model)
        {
            var appUser = await _context.Users.FindAsync(_principal.Name);
            ExceptionHelper.CheckSelfSoft(appUser, "user");

            ExceptionHelper.ThrowIfUserBanned(appUser);

            var superTag = await _context.Tags.FindAsync("") ?? new Tag { Name = "" };

            var op = new OpMessage
            {
                Content = model.Content,
                Title = model.Title,
                SendDate = DateTime.UtcNow,
                Poster = appUser,
                Tags = new[] { new OpMessageTag { Tag = superTag } }
            };
            await _context.OpMessages.AddAsync(op);

            await _context.SaveChangesAsync();

            return op.Id;
        }

        public async Task<OpMessageModel> GetById(int id)
        {
            var opMessage = await _context.OpMessages.FindAsync(id);
            ExceptionHelper.CheckEntitySoft(opMessage, "opMessage");
            return _mapper.Map<OpMessage, OpMessageModel>(opMessage);
        }

        public async Task<IEnumerable<CommentModel>> GetComments(int postId, int page)
        {
            var op = await _context.OpMessages.FindAsync(postId);
            ExceptionHelper.CheckEntitySoft(op, "post");

            return op.Messages.Where(x => !x.IsDeleted)
                     .OrderByDescending(x => x.SendDate)
                     .Select(x => _mapper.Map<Message, CommentModel>(x))
                     .Skip(Constants.PageSize * page)
                     .Take(Constants.PageSize);
        }

        public async Task<bool> PostExists(int postId)
        {
            var post = await _context.OpMessages.FindAsync(postId);
            return post != null && !post.IsDeleted;
        }

        public async Task VotePost(VotePost votePost)
        {
            var appUser = await _context.Users.FindAsync(_principal.Name);
            ExceptionHelper.CheckSelfSoft(appUser, "user");

            var post = await _context.OpMessages.FindAsync(votePost.PostId);
            ExceptionHelper.CheckEntitySoft(post, "post");

            // TODO: validation

            post.Votes.Add(new PostVote
            {
                Voter = appUser,
                IsUpvote = votePost.VoteType switch
                {
                    VoteType.Up => true,
                    VoteType.Down => false,
                    _ => throw new ValidationException("Unknown VoteType value")
                }
            });
            await _context.SaveChangesAsync();
        }

        #endregion
    }
}
