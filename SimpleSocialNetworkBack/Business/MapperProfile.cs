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
            CreateMap<Message, MessageModel>()
                .ReverseMap();

            CreateMap<OpMessage, OpMessageModel>()
                .ReverseMap();

            CreateMap<OpMessageTag, OpMessageTagModel>()
                .ReverseMap();

            CreateMap<SecurePassword, SecurePasswordModel>()
                .ReverseMap();

            CreateMap<Subscription, SubscriptionModel>()
                .ReverseMap();

            CreateMap<Tag, TagModel>()
                .ReverseMap();

            CreateMap<TagBan, TagBanModel>()
                .ReverseMap();

            CreateMap<TagModerator, TagModeratorModel>()
                .ReverseMap();

            CreateMap<User, UserModel>()
                .ReverseMap();
        }
    }
}