using System;
using System.Security.Claims;
using AutoMapper;
using HMA.DTO.Models.User;
using HMA.DTO.ViewModels.User;
using HMA.Infrastructure.Auth;

namespace HMA.AutoMapper.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserInfo, UserInfoViewModel>();
            CreateMap<UserInfo, UserSimpleInfo>();
            CreateMap<UserInfo, UserSimpleInfoViewModel>();

            CreateMap<UserSimpleInfo, UserSimpleInfoViewModel>();

            CreateMap<ClaimsIdentity, UserInfo>()
                .ForMember(
                    dest => dest.GoogleId,
                    opt => opt.MapFrom(
                        src => decimal.Parse(src.FindFirst(ClaimsConstants.NameIdentifier).Value)))
                .ForMember(
                    dest => dest.Email,
                    opt => opt.MapFrom(
                        src => src.FindFirst(ClaimsConstants.EmailAddress).Value))
                .ForMember(
                    dest => dest.IsEmailVerified,
                    opt => opt.MapFrom(
                        src => bool.Parse(src.FindFirst(ClaimsConstants.EmailVerified).Value)))
                .ForMember(
                    dest => dest.PictureUrl,
                    opt => opt.MapFrom(
                        src => new Uri(src.FindFirst(ClaimsConstants.Picture).Value, UriKind.Absolute)))
                .ForMember(
                    dest => dest.GivenName,
                    opt => opt.MapFrom(
                        src => src.FindFirst(ClaimsConstants.GivenName).Value))
                .ForMember(
                    dest => dest.FamilyName,
                    opt => opt.MapFrom(
                        src => src.FindFirst(ClaimsConstants.SurName).Value))
                .ForMember(
                    dest => dest.Locale,
                    opt => opt.MapFrom(
                        src => src.FindFirst(ClaimsConstants.Locale).Value));
        }
    }
}
