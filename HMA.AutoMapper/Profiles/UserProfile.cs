using System;
using System.Security.Claims;
using AutoMapper;
using HMA.DTO.Models;
using HMA.DTO.ViewModels;

namespace HMA.AutoMapper.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserInfo, UserInfoViewModel>()
                .ReverseMap();

            CreateMap<ClaimsIdentity, UserInfo>()
                .ForMember(
                    dest => dest.GoogleId,
                    opt => opt.MapFrom(
                        src => src.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value))
                .ForMember(
                    dest => dest.Email,
                    opt => opt.MapFrom(
                        src => src.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").Value))
                .ForMember(
                    dest => dest.EmailVerified,
                    opt => opt.MapFrom(
                        src => bool.Parse(src.FindFirst("email_verified").Value)))
                .ForMember(
                    dest => dest.Picture,
                    opt => opt.MapFrom(
                        src => new Uri(src.FindFirst("picture").Value)))
                .ForMember(
                    dest => dest.GivenName,
                    opt => opt.MapFrom(src => src.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname").Value))
                .ForMember(
                    dest => dest.FamilyName,
                    opt => opt.MapFrom(src => src.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname").Value))
                .ForMember(
                    dest => dest.Locale,
                    opt => opt.MapFrom(src => src.FindFirst("locale").Value));
        }
    }
}
