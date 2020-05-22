﻿using HMA.DTO.Models.Invite;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HMA.BLL.Tier1.Exceptions.House;
using HMA.BLL.Tier1.Exceptions.Invite;
using HMA.BLL.Tier1.Exceptions.User;

namespace HMA.BLL.Tier2.Services.Interfaces
{
    public interface IHouseInviteT2Service
    {
        /// <summary>
        /// Get all invites for provided user
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task<List<HouseInviteInfo>> GetInvitesByUserIdAsync(
            decimal userId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Create invite 
        /// </summary>
        /// <param name="houseInviteCreationRequest">House invite creation request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <exception cref="TooManyHouseInvitesException"></exception>
        /// <exception cref="HouseNotFoundException"></exception>
        /// <exception cref="UserNotFoundException"></exception>
        Task<HouseInviteInfo> CreateInviteAsync(
            HouseInviteCreationRequest houseInviteCreationRequest,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Accept invite for provided user
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="inviteId">Invite Id</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <exception cref="HouseInviteNotFoundException"></exception>
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
        /// <exception cref="HouseInviteNotFoundException"></exception>
        Task DeclineInviteAsync(
            decimal userId,
            BsonObjectId inviteId,
            CancellationToken cancellationToken = default);
    }
}
