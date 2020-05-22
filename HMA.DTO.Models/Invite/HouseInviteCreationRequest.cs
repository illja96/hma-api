using MongoDB.Bson;

namespace HMA.DTO.Models.Invite
{
    public class HouseInviteCreationRequest
    {
        /// <summary>
        /// House id
        /// </summary>
        public BsonObjectId HouseId { get; set; }

        /// <summary>
        /// User id how invited.
        /// Should be an house owner
        /// </summary>
        public decimal InvitedByUserId { get; set; }

        /// <summary>
        /// User email of invited user
        /// </summary>
        public string UserEmail { get; set; }
    }
}
