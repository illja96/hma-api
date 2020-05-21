using MongoDB.Bson;

namespace HMA.DTO.Models.Invite
{
    public class HouseInviteCreationRequest
    {
        public BsonObjectId HouseId { get; set; }

        public decimal InvitedByUserId { get; set; }

        public string UserEmail { get; set; }
    }
}
