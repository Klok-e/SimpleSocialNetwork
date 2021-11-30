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

namespace Business.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly SocialDbContext _context;
        private readonly IMapper _mapper;
        private readonly TypedClaimsPrincipal _principal;

        public UserService(SocialDbContext context, IMapper mapper, TypedClaimsPrincipal principal)
        {
            _context = context;
            _mapper = mapper;
            _principal = principal;
        }

        #region IUserService Members

        public async Task<UserModel> GetUser(string login)
        {
            var user = await _context.Users.FindAsync(login);
            ExceptionHelper.CheckEntitySoft(user, "user");
            if (_principal.Name != login && _principal.Role != Roles.Admin)
                throw new ForbiddenException("You can't get full info about another user");

            return _mapper.Map<ApplicationUser, UserModel>(user);
        }

        public async Task<LimitedUserModel> GetUserLimited(string login)
        {
            var user = await _context.Users.FindAsync(login);
            ExceptionHelper.CheckEntitySoft(user, "user");

            return _mapper.Map<ApplicationUser, LimitedUserModel>(user);
        }

        public async Task<bool> UserExists(string login)
        {
            var user = await _context.Users.FindAsync(login);
            // deleted users should still be regarded as existing
            return user != null;
        }

        public async Task ChangeUserInfo(ChangeUserInfo changeInfo)
        {
            var user = await _context.Users.FindAsync(_principal.Name);
            ExceptionHelper.CheckSelfSoft(user, "user");

            user.About = changeInfo.About;
            user.DateBirth = changeInfo.DateBirth;

            await _context.SaveChangesAsync();
        }

        public Task<IEnumerable<LimitedUserModel>> SearchUsers(SearchUsersModel search)
        {
            return Task.FromResult(_context.Users.Where(x => !x.IsDeleted)
                                           .OrderBy(x => x.Login)
                                           .AsEnumerable()
                                           .Where(u =>
                                           {
                                               var name = true;
                                               if (!string.IsNullOrEmpty(search.NamePattern))
                                                   name = FuzzySearch.FuzzyMatch(u.Login, search.NamePattern);

                                               var about = true;
                                               if (u.About == null && !string.IsNullOrEmpty(search.AboutPattern))
                                                   about = false;
                                               else if (string.IsNullOrEmpty(search.AboutPattern))
                                                   about = true;
                                               else if (!string.IsNullOrEmpty(search.AboutPattern) && u.About != null)
                                                   about = FuzzySearch.FuzzyMatch(u.About, search.AboutPattern);

                                               return name && about;
                                           })
                                           .Select(x => _mapper.Map<ApplicationUser, LimitedUserModel>(x)));
        }

        public async Task BanUser(BanUserModel ban)
        {
            var (banInitiator, user) = await AdminAndTarget(_principal, ban.Login);

            var superTag = await _context.Tags.FindAsync("") ?? new Tag { Name = "" };

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

        public async Task LiftBanFromUser(string login)
        {
            var (_, user) = await AdminAndTarget(_principal, login);

            // cancel all not cancelled and not expired bans
            foreach (var tagBan in user.BansReceived.Where(x => !x.Cancelled && x.ExpirationDate > DateTime.UtcNow))
                tagBan.Cancelled = true;

            await _context.SaveChangesAsync();
        }

        public async Task<bool> UserDeleted(string login)
        {
            var user = await _context.Users.FindAsync(login);
            if (user == null)
                throw new ValidationException("Nonexistent user");

            return user.IsDeleted;
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

        public async Task<bool> UserBanned(string login)
        {
            var user = await _context.Users.FindAsync(login);
            ExceptionHelper.CheckEntitySoft(user, "user login");

            if (_principal.Role != Roles.Admin && _principal.Name != login)
                throw new ForbiddenException("No rights: either not an admin or not same user");

            return user.BansReceived.Any(ban => !ban.Cancelled && ban.ExpirationDate > DateTime.UtcNow);
        }

        #endregion

        private async Task<(ApplicationUser adm, ApplicationUser target)> AdminAndTarget(TypedClaimsPrincipal adm,
            string targLogin)
        {
            var admin = await _context.Users.FindAsync(adm.Name);
            var targetUser = await _context.Users.FindAsync(targLogin);

            ExceptionHelper.CheckSelfSoft(admin, "admin");
            if (adm.Role != Roles.Admin)
                throw new ForbiddenException("No rights");

            ExceptionHelper.CheckEntitySoft(targetUser, "user");

            return (admin, targetUser);
        }
    }
}
