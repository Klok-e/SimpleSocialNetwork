using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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

        public OpMessageService(SocialDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Task<IEnumerable<OpMessageModel>> GetAll()
        {
            return Task.FromResult(_context.OpMessages
                .OrderByDescending(x => x.SendDate)
                .Select(x => _mapper.Map<OpMessage, OpMessageModel>(x))
                .AsEnumerable());
        }

        public async Task<int> MakeAPost(string user, CreateOpMessageModel model)
        {
            var appUser = await _context.Users.FindAsync(user);
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
            return op.Messages
                .OrderByDescending(x => x.SendDate)
                .Select(x => _mapper.Map<Message, CommentModel>(x));
        }
    }
}