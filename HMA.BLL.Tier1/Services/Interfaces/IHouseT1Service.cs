using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HMA.BLL.Tier1.Exceptions.House;
using HMA.DTO.Models.House;
using MongoDB.Bson;

namespace HMA.BLL.Tier1.Services.Interfaces
{
    /// <summary>
    /// House server
    /// </summary>
    public interface IHouseT1Service
    {
        /// <summary>
        /// Get available house info by id for provided user
        /// </summary>
        /// <param name="houseId">House id</param>
        /// <param name="userId">User id</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task<HouseInfo> GetHouseInfoByIdAvailableForUserAsync(
            BsonObjectId houseId,
            decimal userId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get available house simple info by id for provided user
        /// </summary>
        /// <param name="houseId">House id</param>
        /// <param name="userId">User id</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <exception cref="HouseNotFoundException"></exception>
        Task<HouseSimpleInfo> GetSimpleHouseInfoByIdAvailableForUserAsync(
            BsonObjectId houseId,
            decimal userId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get  house simple infos by ids
        /// </summary>
        /// <param name="houseIds">House ids</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        Task<List<HouseSimpleInfo>> GetSimpleHouseInfosByIdsAsync(
            List<BsonObjectId> houseIds,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get owned simple house infos of provided user
        /// </summary>
        /// <param name="ownerId">Owner (user) id</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task<List<HouseSimpleInfo>> GetOwnedHouseSimpleInfosOfUserAsync(
            decimal ownerId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get membership house simple infos of provided user
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task<List<HouseSimpleInfo>> GetMembershipHouseSimpleInfosOfUserAsync(
            decimal userId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Create new house
        /// </summary>
        /// <param name="houseCreationRequest">House creation request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task<HouseInfo> CreateHouseAsync(
            HouseCreationRequest houseCreationRequest,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete house by id if provided user is an owner.
        /// Leave house by id if provided user is a member
        /// </summary>
        /// <param name="houseId">House id</param>
        /// <param name="userId">User id</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <exception cref="HouseNotFoundException"></exception>
        Task DeleteOrLeaveHouseByIdAvailableForUserAsync(
            BsonObjectId houseId,
            decimal userId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Remove house member by id if provided user is an house owner
        /// </summary>
        /// <param name="houseId">House id</param>
        /// <param name="userId">User id</param>
        /// <param name="memberId">Member id</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <exception cref="HouseNotFoundException"></exception>
        Task RemoveHouseMemberByIdAvailableForUserAsync(
            BsonObjectId houseId,
            decimal userId,
            decimal memberId,
            CancellationToken cancellationToken = default);
    }
}
