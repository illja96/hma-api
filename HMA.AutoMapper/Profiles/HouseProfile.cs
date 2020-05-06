using AutoMapper;
using HMA.DTO.Models.House;
using HMA.DTO.ViewModels.House;

namespace HMA.AutoMapper.Profiles
{
    public class HouseProfile : Profile
    {
        public HouseProfile()
        {
            CreateMap<AvailableHousesInfo, AvailableHousesInfoViewModel>();
            
            CreateMap<HouseInfo, HouseSimpleInfo>();
            CreateMap<HouseInfo, HouseInfoViewModel>();

            CreateMap<HouseSimpleInfo, HouseSimpleInfoViewModel>();

            CreateMap<HouseCreationRequestViewModel, HouseCreationRequest>();
            CreateMap<HouseCreationRequest, HouseInfo>();
        }
    }
}
