using HMA.DTO.Models.Invite;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HMA.BLL.Tier1.Exceptions.House;
using HMA.BLL.Tier1.Exceptions.Invite;

namespace HMA.BLL.Tier2.Services.Interfaces
{
    public interface IHouseInviteT2Service
    {
        /// <summary>
        /// Get available invites for provided user
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task<List<HouseInviteSimpleInfo>> GetInvitesAvailableForUserAsync(
            decimal userId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Create invite 
        /// </summary>
        /// <param name="houseInviteCreationRequest">House invite creation request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <exception cref="TooManyHouseInvitesException"></exception>
        /// <exception cref="HouseNotFoundException"></exception>
        Task<HouseInviteSimpleInfo> CreateInviteAsync(
            HouseInviteCreationRequest houseInviteCreationRequest,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Accept invite available for provided user
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="inviteId">Invite Id</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <exception cref="HouseInviteNotFoundException"></exception>
        Task AcceptInviteAvailableForUserAsync(
            decimal userId,
            BsonObjectId inviteId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Decline invite available for provided user
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="inviteId">Invite Id</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <exception cref="HouseInviteNotFoundException"></exception>
        Task DeclineInviteAvailableForUserAsync(
            decimal userId,
            BsonObjectId inviteId,
            CancellationToken cancellationToken = default);
    }
}
