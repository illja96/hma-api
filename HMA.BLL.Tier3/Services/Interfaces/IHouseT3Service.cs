﻿using System.Threading;
using System.Threading.Tasks;
using HMA.DTO.ViewModels.House;
using Microsoft.AspNetCore.Mvc;

namespace HMA.BLL.Tier3.Services.Interfaces
{
    /// <summary>
    /// House service
    /// </summary>
    public interface IHouseT3Service
    {
        /// <summary>
        /// Get houses available for current user
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        Task<ObjectResult> GetHousesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Get house by id.
        /// Returns house only if house is available to current user
        /// </summary>
        /// <param name="houseId">House id</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task<ObjectResult> GetHouseByIdAsync(
            string houseId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Create new house
        /// </summary>
        /// <param name="houseCreationRequestViewModel">House creation request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task<ObjectResult> CreateHouseAsync(
            HouseCreationRequestViewModel houseCreationRequestViewModel,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete house by id if provided user is an owner.
        /// Leave house by id if provided user is a member
        /// </summary>
        /// <param name="houseId">House id</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task<ObjectResult> DeleteOrLeaveHouseByIdAsync(
            string houseId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Remove member by id
        /// Remove house member only if current user is house owner.
        /// </summary>
        /// <param name="houseId">House id</param>
        /// <param name="memberId">Member id</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        Task<ObjectResult> RemoveHouseMemberByIdAsync(
            string houseId,
            decimal memberId,
            CancellationToken cancellationToken = default);
    }
}
