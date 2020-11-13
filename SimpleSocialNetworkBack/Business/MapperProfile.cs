using System;
using AutoMapper;
using Business.Models;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Business
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<ApplicationUser, UserModel>()
                .ReverseMap();
            
            CreateMap<OpMessage, OpMessageModel>()
                .ReverseMap();
        }
    }
}