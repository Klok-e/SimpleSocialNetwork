using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.Common;
using Business.Models.Responses;
using Business.Validation;
using DataAccess;
using DataAccess.Entities;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Business.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly AppSettings _appSettings;
        private readonly SocialDbContext _context;
        private readonly IMapper _mapper;

        public AuthService(SocialDbContext context, IMapper mapper, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        public async Task<UserModel> Register(string login, string password)
        {
            // check whether user exists
            var exists = await _context.Users!.FindAsync(login);
            if (exists != null)
                throw new ValidationException("Login is already taken");

            // if first user ever, make admin
            var isAdmin = !_context.Users.Any();

            var user = new ApplicationUser
            {
                Login = login,
                Password = HashPassword(password),
                IsAdmin = isAdmin
            };

            await _context.Users!.AddAsync(user);
            await _context.SaveChangesAsync();

            return _mapper.Map<ApplicationUser, UserModel>(user);
        }

        public async Task<LoggedInUser> Login(string login, string password)
        {
            // check whether user exists
            var user = await _context.Users.FindAsync(login);
            ExceptionHelper.CheckEntitySoft(user, "user");

            if (user.Password == null)
                throw new ArgumentNullException(nameof(user.Password),
                    "Password somehow was null idk");
            if (!CheckPasswordHash(user.Password.Salt, user.Password.Hashed, password))
                throw new ValidationException("Wrong password");

            var role = user.IsAdmin ? Roles.Admin : Roles.User;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Login),
                    new Claim(ClaimTypes.Role, role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenStr = tokenHandler.WriteToken(token);

            return new LoggedInUser
            {
                Login = user.Login,
                Token = tokenStr,
                Role = role
            };
        }

        private static bool CheckPasswordHash(string salt, string hash, string passwordToCheck)
        {
            var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                passwordToCheck,
                Convert.FromBase64String(salt),
                KeyDerivationPrf.HMACSHA1,
                10000,
                256 / 8));

            return hashed == hash;
        }

        /// <summary>
        /// </summary>
        /// <param name="password"></param>
        /// <returns>SecurePassword with salt and hash set</returns>
        private static SecurePassword HashPassword(string password)
        {
            // code stolen from here
            // https://docs.microsoft.com/en-us/aspnet/core/security/data-protection/consumer-apis/password-hashing?view=aspnetcore-5.0

            // generate a 128-bit salt using a secure PRNG
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password,
                salt,
                KeyDerivationPrf.HMACSHA1,
                10000,
                256 / 8));
            return new SecurePassword {Hashed = hashed, Salt = Convert.ToBase64String(salt)};
        }
    }
}