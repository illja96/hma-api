using System.Collections.Generic;
using MongoDB.Bson;

namespace HMA.DTO.Models.House
{
    public class HouseMembersUpdateRequest
    {
        public BsonObjectId HouseId { get; set; }

        public decimal UserId { get; set; }

        public List<decimal> MemberIds { get; set; }
    }
}
