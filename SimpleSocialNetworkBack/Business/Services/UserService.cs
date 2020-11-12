using System;
using System.Threading.Tasks;
using AutoMapper;
using Business.Interfaces;
using Business.Models;
using DataAccess;
using DataAccess.Entities;
using System.Security.Cryptography;
using System.Text;
using Business.Validation;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Business.Services
{
    public class UserService : IUserService
    {
        private readonly SocialDbContext _dbContext;
        private readonly IMapper _mapper;

        public UserService(SocialDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<UserModel> Register(string login, string password)
        {
            // check whether user exists
            var exists = await _dbContext.Users!.FindAsync(login);
            if (exists != null)
                throw new SocialException("Login is already taken");

            var user = new User {Login = login};
            await _dbContext.Users!.AddAsync(user);

            var secure = HashPassword(password);
            user.Password = secure;

            await _dbContext.SaveChangesAsync();

            return _mapper.Map<User, UserModel>(user);
        }

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