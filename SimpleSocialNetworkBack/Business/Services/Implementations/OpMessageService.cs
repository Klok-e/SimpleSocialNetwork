using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Models;
using Business.Models.Answers;
using Business.Models.Requests;
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
            return Task.FromResult(_context.OpMessages.Select(x => _mapper.Map<OpMessage, OpMessageModel>(x))
                .AsEnumerable());
        }

        public async Task<OpMessageModel> MakeAPost(UserModel user, CreateOpMessageModel model)
        {
            var op = new OpMessage
            {
                Content = model.Content,
                Title = model.Title, //model.Tags.Select(x => new OpMessageTag { }
                SendDate = DateTime.UtcNow,
            };
            await _context.OpMessages.AddAsync(op);

            await _context.SaveChangesAsync();

            return _mapper.Map<OpMessage, OpMessageModel>(op);
        }
    }
}