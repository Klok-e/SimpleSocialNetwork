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
    public class UserService : IUserService
    {
        private readonly SocialDbContext _context;
        private readonly IMapper _mapper;
        private readonly TypedClaimsPrincipal _principal;

        public UserService(
            SocialDbContext context,
            IMapper mapper,
            TypedClaimsPrincipal principal)
        {
            _context = context;
            _mapper = mapper;
            _principal = principal;
        }

        public async Task<UserModel> GetUser(string login)
        {
            var user = await _context.Users.FindAsync(login);
            if (user == null)
                throw new ValidationException("Nonexistent login");
            if (_principal.Name != login)
                throw new ForbiddenException("You can't get full info about another user");

            return _mapper.Map<ApplicationUser, UserModel>(user);
        }

        public async Task<LimitedUserModel> GetUserLimited(string login)
        {
            var user = await _context.Users.FindAsync(login);
            if (user == null)
                throw new ValidationException("Nonexistent login");

            return _mapper.Map<ApplicationUser, LimitedUserModel>(user);
        }

        public async Task<bool> UserExists(string login)
        {
            return await _context.Users.FindAsync(login) != null;
        }

        public async Task ChangeUserInfo(ChangeUserInfo changeInfo)
        {
            var user = await _context.Users.FindAsync(_principal.Name);
            if (user == null)
                throw new BadCredentialsException("Nonexistent user");

            user.About = changeInfo.About;
            user.DateBirth = changeInfo.DateBirth;

            await _context.SaveChangesAsync();
        }

        public Task<IEnumerable<LimitedUserModel>> SearchUsers(SearchUsersModel search)
        {
            return Task.FromResult(
                _context.Users
                    .AsEnumerable()
                    .Where(u =>
                    {
                        var name = true;
                        if (!string.IsNullOrEmpty(search.NamePattern))
                            name = FuzzySearch.FuzzyMatch(u.Login, search.NamePattern);

                        var about = true;
                        if (u.About == null)
                            about = false;
                        else if (!string.IsNullOrEmpty(search.AboutPattern))
                            about = FuzzySearch.FuzzyMatch(u.About, search.AboutPattern);

                        return name && about;
                    })
                    .Select(x => _mapper.Map<ApplicationUser, LimitedUserModel>(x))
            );
        }
    }
}