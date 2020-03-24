using AutoMapper;
using HMA.DTO.Models.House;
using HMA.DTO.ViewModels.House;

namespace HMA.AutoMapper.Profiles
{
    public class HouseProfile : Profile
    {
        public HouseProfile()
        {
            CreateMap<HouseSimpleInfo, HouseSimpleInfoViewModel>()
                .ReverseMap();

            CreateMap<HouseInfo, HouseInfoViewModel>()
                .ReverseMap();

            CreateMap<HouseCreationRequestViewModel, HouseCreationRequest>();
            CreateMap<HouseCreationRequest, HouseInfo>();
        }
    }
}
