using System;
using AutoMapper;
using Business.Models;
using Business.Models.Responses;
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

            CreateMap<Message, CommentModel>()
                .ReverseMap();

            CreateMap<OpMessage, OpMessageModel>()
                .ReverseMap();
        }
    }
}