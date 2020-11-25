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
            if (user.IsDeleted)
                throw new ValidationException("User was deleted");
            if (_principal.Name != login && _principal.Role != Roles.Admin)
                throw new ForbiddenException("You can't get full info about another user");

            return _mapper.Map<ApplicationUser, UserModel>(user);
        }

        public async Task<LimitedUserModel> GetUserLimited(string login)
        {
            var user = await _context.Users.FindAsync(login);
            if (user == null)
                throw new ValidationException("Nonexistent login");
            if (user.IsDeleted)
                throw new ValidationException("User was deleted");

            return _mapper.Map<ApplicationUser, LimitedUserModel>(user);
        }

        public async Task<bool> UserExists(string login)
        {
            var user = await _context.Users.FindAsync(login);
            // deleted users should still be regarded to as existing
            return user != null;
        }

        public async Task ChangeUserInfo(ChangeUserInfo changeInfo)
        {
            var user = await _context.Users.FindAsync(_principal.Name);
            if (user == null)
                throw new BadCredentialsException("Nonexistent user");
            if (user.IsDeleted)
                throw new ValidationException("User was deleted");

            user.About = changeInfo.About;
            user.DateBirth = changeInfo.DateBirth;

            await _context.SaveChangesAsync();
        }

        public Task<IEnumerable<LimitedUserModel>> SearchUsers(SearchUsersModel search)
        {
            return Task.FromResult(
                _context.Users
                    .Where(x => !x.IsDeleted)
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

        public async Task BanUser(BanUserModel ban)
        {
            var (banInitiator, user) = await AdminAndTarget(_principal, ban.Login);

            var superTag = await _context.Tags.FindAsync("") ?? new Tag {Name = ""};

            await _context.TagBans.AddAsync(new TagBan
            {
                Moderator = banInitiator,
                User = user,
                Tag = superTag,
                ExpirationDate = ban.ExpirationDate,
                BanIssuedDate = DateTime.UtcNow
            });
            await _context.SaveChangesAsync();
        }

        public async Task ElevateUser(string login)
        {
            var (_, user) = await AdminAndTarget(_principal, login);

            user.IsAdmin = true;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserSoft(string login)
        {
            var (_, user) = await AdminAndTarget(_principal, login);

            user.IsDeleted = true;

            await _context.SaveChangesAsync();
        }

        private async Task<(ApplicationUser adm, ApplicationUser target)> AdminAndTarget(TypedClaimsPrincipal adm,
            string targLogin)
        {
            var admin = await _context.Users.FindAsync(adm.Name);
            if (admin == null)
                throw new BadCredentialsException("Nonexistent user");
            if (adm.Role != Roles.Admin)
                throw new ForbiddenException("No rights");

            var user = await _context.Users.FindAsync(targLogin);
            if (user == null)
                throw new ValidationException("Nonexistent login");

            return (admin, user);
        }

        public async Task<bool> UserBanned(string login)
        {
            var user = await _context.Users.FindAsync(login);
            if (user == null)
                throw new ValidationException("Nonexistent login");
            if (_principal.Role != Roles.Admin && _principal.Name != login)
                throw new ForbiddenException("No rights: either not an admin or not same user");

            return user.BansReceived
                .Any(ban => !ban.Cancelled && ban.ExpirationDate > DateTime.UtcNow);
        }
    }
}