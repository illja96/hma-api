using HMA.DTO.ViewModels.House;
using HMA.DTO.ViewModels.User;
using System;

namespace HMA.DTO.ViewModels.Invite
{
    public class HouseInviteSimpleInfoViewModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// House info
        /// </summary>
        public HouseSimpleInfoViewModel HouseInfo { get; set; }

        /// <summary>
        /// Invited by
        /// </summary>
        public UserSimpleInfoViewModel InvitedBy { get; set; }

        /// <summary>
        /// Created at
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}
