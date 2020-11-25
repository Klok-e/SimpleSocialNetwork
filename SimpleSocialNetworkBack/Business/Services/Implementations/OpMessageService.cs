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

        public OpMessageService(
            SocialDbContext context,
            IMapper mapper,
            TypedClaimsPrincipal principal)
        {
            _context = context;
            _mapper = mapper;
            _principal = principal;
        }

        public Task<IEnumerable<OpMessageModel>> GetAll()
        {
            return Task.FromResult(_context.OpMessages
                // include to prevent "There is already an open DataReader associated with this Connection which must be closed first."
                // error by preloading poster
                .Include(x => x.Poster)
                .Where(x => !x.IsDeleted)
                .OrderByDescending(x => x.SendDate)
                .Select(x => _mapper.Map<OpMessage, OpMessageModel>(x))
                .AsEnumerable());
        }

        public async Task<IEnumerable<OpMessageModel>> PostsFromUser(string login)
        {
            var user = await _context.Users.FindAsync(login);
            ExceptionHelper.CheckEntitySoft(user, "user");

            return user.Posts
                .Where(x => !x.IsDeleted)
                .Select(x => _mapper.Map<OpMessage, OpMessageModel>(x));
        }

        public async Task<int> MakeAPost(CreateOpMessageModel model)
        {
            var appUser = await _context.Users.FindAsync(_principal.Name);
            ExceptionHelper.CheckSelfSoft(appUser, "user");

            var superTag = await _context.Tags.FindAsync("") ?? new Tag {Name = ""};

            var op = new OpMessage
            {
                Content = model.Content,
                Title = model.Title,
                SendDate = DateTime.UtcNow,
                Poster = appUser,
                Tags = new[] {new OpMessageTag {Tag = superTag}}
            };
            await _context.OpMessages.AddAsync(op);

            await _context.SaveChangesAsync();

            return op.Id;
        }

        public async Task<OpMessageModel> GetById(int id)
        {
            var opMessage = await _context.OpMessages.FindAsync(id);
            if (opMessage.IsDeleted)
                throw new ValidationException("Post was deleted");
            return _mapper.Map<OpMessage, OpMessageModel>(opMessage);
        }

        public async Task<IEnumerable<CommentModel>> GetComments(int postId)
        {
            var op = await _context.OpMessages.FindAsync(postId);
            ExceptionHelper.CheckEntitySoft(op, "post");

            return op.Messages
                .Where(x => !x.IsDeleted)
                .OrderByDescending(x => x.SendDate)
                .Select(x => _mapper.Map<Message, CommentModel>(x));
        }

        public async Task<bool> PostExists(int postId)
        {
            var post = await _context.OpMessages.FindAsync(postId);
            return post != null && !post.IsDeleted;
        }

        public async Task VotePost(VotePost votePost)
        {
            var post = await _context.OpMessages.FindAsync(votePost.PostId);
            ExceptionHelper.CheckEntitySoft(post, "post");

            post.Points += votePost.VoteType switch
            {
                VoteType.Up => 1,
                VoteType.Down => -1,
                _ => throw new ValidationException("Unknown VoteType value")
            };
            await _context.SaveChangesAsync();
        }
    }
}