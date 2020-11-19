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
            CreateMap<ApplicationUser, UserModel>();
            CreateMap<ApplicationUser, LimitedUserModel>();

            CreateMap<Message, CommentModel>()
                .ForMember(dest => dest.PosterId,
                    opt => 
                        opt.MapFrom(src => src.Poster.Login));

            CreateMap<OpMessage, OpMessageModel>()
                .ForMember(dest => dest.PosterId,
                    opt =>
                        opt.MapFrom(src => src.Poster!.Login));

            CreateMap<Subscription, SubscriptionModel>();
        }
    }
}