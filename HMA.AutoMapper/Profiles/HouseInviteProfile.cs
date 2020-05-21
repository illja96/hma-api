using AutoMapper;
using HMA.DTO.Models.Invite;
using HMA.DTO.ViewModels.Invite;

namespace HMA.AutoMapper.Profiles
{
    public class HouseInviteProfile : Profile
    {
        public HouseInviteProfile()
        {
            CreateMap<HouseInviteCreationRequestViewModel, HouseInviteCreationRequest>();

            CreateMap<HouseInviteInfo, HouseInviteSimpleInfo>();
            CreateMap<HouseInviteSimpleInfo, HouseInviteSimpleInfoViewModel>();
        }
    }
}
