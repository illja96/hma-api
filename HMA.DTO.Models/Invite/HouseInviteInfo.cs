using HMA.DTO.Models.Base;
using MongoDB.Bson;
using System;

namespace HMA.DTO.Models.Invite
{
    public class HouseInviteInfo : BaseDalModel
    {
        public BsonObjectId HouseId { get; set; }

        public decimal InvitedByUserId { get; set; }

        public string UserEmail { get; set; }

        public decimal UserId { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
