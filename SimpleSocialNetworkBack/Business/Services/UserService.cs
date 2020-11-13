using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Business.Interfaces;
using Business.Models;
using DataAccess;
using DataAccess.Entities;
using System.Security.Cryptography;
using System.Text;
using Business.Common;
using Business.Validation;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Business.Services
{
    public class UserService : IUserService
    {
        private readonly SocialDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;

        public UserService(SocialDbContext dbContext, IMapper mapper, IOptions<AppSettings> appSettings)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        public async Task<UserModel> Register(string login, string password)
        {
            // check whether user exists
            var exists = await _dbContext.Users!.FindAsync(login);
            if (exists != null)
                throw new SocialException("Login is already taken");

            var user = new ApplicationUser {Login = login};
            await _dbContext.Users!.AddAsync(user);

            user.Password = HashPassword(password);

            await _dbContext.SaveChangesAsync();

            return _mapper.Map<ApplicationUser, UserModel>(user);
        }

        public async Task<LoggedInUser> Login(string login, string password)
        {
            // check whether user exists
            var user = await _dbContext.Users!.FindAsync(login);
            if (user == null)
                throw new SocialException("Nonexistent login");


            if (!CheckPasswordHash(user.Password!.Salt, user.Password!.Hashed, password))
                throw new SocialException("Wrong password");

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Login),
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
                Token = tokenStr
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
        /// 
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