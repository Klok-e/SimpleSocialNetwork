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
            CreateMap<ApplicationUser, LimitedUserModel>()
                .ReverseMap();

            CreateMap<Message, CommentModel>()
                .ForMember(dest => dest.PosterId,
                    opt => opt.MapFrom(src => src.Poster.Login))
                .ReverseMap();

            CreateMap<OpMessage, OpMessageModel>()
                .ReverseMap();
        }
    }
}