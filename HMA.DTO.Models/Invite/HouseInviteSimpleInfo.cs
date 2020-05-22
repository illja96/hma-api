using HMA.DTO.Models.Base;
using HMA.DTO.Models.House;
using HMA.DTO.Models.User;
using MongoDB.Bson;
using System;

namespace HMA.DTO.Models.Invite
{
    public class HouseInviteSimpleInfo : BaseDalModel
    {
        public BsonObjectId HouseId { get; set; }

        public HouseSimpleInfo HouseInfo { get; set; }

        public decimal InvitedByUserId { get; set; }

        public UserSimpleInfo InvitedBy { get; set; }

        public string UserEmail { get; set; }

        public decimal UserId { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
