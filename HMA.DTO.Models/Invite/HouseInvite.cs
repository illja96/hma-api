using HMA.DTO.Models.Base;
using MongoDB.Bson;
using System;

namespace HMA.DTO.Models.Invite
{
    public class HouseInvite : BaseDalModel
    {
        public BsonObjectId HouseId { get; set; }

        public string UserEmail { get; set; }

        public decimal UserId { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
