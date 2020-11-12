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
    }
}