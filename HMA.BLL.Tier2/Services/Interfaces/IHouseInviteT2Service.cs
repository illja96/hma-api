using HMA.DTO.Models.Invite;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HMA.BLL.Tier2.Services.Interfaces
{
    public interface IHouseInviteT2Service
    {
        /// <summary>
        /// Get all invites for provided user
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task<List<HouseInviteInfo>> GetInvitesAsync(
            decimal userId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Create invite 
        /// </summary>
        /// <param name="houseInviteCreationRequest">House invite creation request</param>
        /// <param name="cancellationToken"></param>
        Task<HouseInviteInfo> CreateInviteAsync(
            HouseInviteCreationRequest houseInviteCreationRequest,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Accept invite for provided user
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="inviteId">Invite Id</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task AcceptInviteAsync(
            decimal userId,
            BsonObjectId inviteId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Decline invite for provided user
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="inviteId">Invite Id</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task DeclineInviteAsync(
            decimal userId,
            BsonObjectId inviteId,
            CancellationToken cancellationToken = default);
    }
}
