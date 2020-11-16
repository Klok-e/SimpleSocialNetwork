using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Business.Models;
using Business.Models.Responses;
using DataAccess;
using DataAccess.Entities;

namespace Business.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly SocialDbContext _context;
        private readonly IMapper _mapper;

        public UserService(SocialDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserModel> Get(string login)
        {
            var user = await _context.Users.FindAsync(login);

            return _mapper.Map<ApplicationUser, UserModel>(user);
        }

        public async Task<bool> UserExists(string login)
        {
            return await _context.Users.FindAsync(login) != null;
        }
    }
}