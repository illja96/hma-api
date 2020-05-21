using HMA.DTO.ViewModels.House;
using HMA.DTO.ViewModels.User;
using System;

namespace HMA.DTO.ViewModels.Invite
{
    public class HouseInviteSimpleInfoViewModel
    {
        public string Id { get; set; }

        public HouseSimpleInfoViewModel House { get; set; }

        public UserSimpleInfoViewModel InvitedBy { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
