using System.Linq;
using AutoMapper;
using Business.Models.Responses;
using DataAccess.Entities;

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
                        opt.MapFrom(src => src.Poster == null ? null : src.Poster.Login));

            CreateMap<OpMessage, OpMessageModel>()
                .ForMember(dest => dest.PosterId,
                    opt =>
                        opt.MapFrom(src => src.Poster == null ? null : src.Poster.Login))
                .ForMember(dest => dest.Points,
                    opt =>
                        opt.MapFrom(src => src
                            .Votes.Select(x => x.IsUpvote ? 1 : -1).Sum()));

            CreateMap<Subscription, SubscriptionModel>()
                .ForMember(dest => dest.IsActive,
                    opt =>
                        opt.MapFrom(src => !src.IsNotActive));
        }
    }
}