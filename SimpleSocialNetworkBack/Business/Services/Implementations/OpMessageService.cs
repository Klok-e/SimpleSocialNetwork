using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Common;
using Business.Models;
using Business.Models.Requests;
using Business.Models.Responses;
using Business.Validation;
using DataAccess;
using DataAccess.Entities;

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
                .OrderByDescending(x => x.SendDate)
                .Select(x => _mapper.Map<OpMessage, OpMessageModel>(x))
                .AsEnumerable());
        }

        public async Task<int> MakeAPost(CreateOpMessageModel model)
        {
            var appUser = await _context.Users.FindAsync(_principal.Name);
            if (appUser == null)
                throw new BadCredentialsException("Nonexistent user");
            var op = new OpMessage
            {
                Content = model.Content,
                Title = model.Title,
                SendDate = DateTime.UtcNow,
                Poster = appUser,
                Tags = new List<OpMessageTag>()
            };
            await _context.OpMessages.AddAsync(op);

            await _context.SaveChangesAsync();

            return op.Id;
        }

        public async Task<OpMessageModel> GetById(int id)
        {
            var opMessage = await _context.OpMessages.FindAsync(id);
            return _mapper.Map<OpMessage, OpMessageModel>(opMessage);
        }

        public async Task<IEnumerable<CommentModel>> GetComments(int postId)
        {
            var op = await _context.OpMessages.FindAsync(postId);
            if (op == null)
                throw new ValidationException("Nonexistent postId");
            return op.Messages
                .OrderByDescending(x => x.SendDate)
                .Select(x => _mapper.Map<Message, CommentModel>(x));
        }

        public async Task<bool> PostExists(int postId)
        {
            return await _context.OpMessages.FindAsync(postId) != null;
        }

        public async Task VotePost(VotePost votePost)
        {
            var post = await _context.OpMessages.FindAsync(votePost.PostId);
            if (post == null)
                throw new ValidationException("Nonexistent post");

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