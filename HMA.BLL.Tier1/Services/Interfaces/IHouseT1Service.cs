using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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
        /// Create new house
        /// </summary>
        /// <param name="houseCreationRequest">House creation request</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<HouseInfo> CreateHouseAsync(
            HouseCreationRequest houseCreationRequest,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get available house info by id for provided user
        /// </summary>
        /// <param name="houseId">House id</param>
        /// <param name="userId">User id</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<HouseInfo> GetHouseInfoByIdAsync(
            BsonObjectId houseId,
            decimal userId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get available house simple info by id for provided user
        /// </summary>
        /// <param name="houseId">House id</param>
        /// <param name="userId">User id</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<HouseSimpleInfo> GetSimpleHouseByIdAsync(
            BsonObjectId houseId,
            decimal userId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get owned houses for provided user
        /// </summary>
        /// <param name="ownerId">Owner (user) id</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<HouseSimpleInfo>> GetOwnedHouseInfosAsync(
            decimal ownerId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get membership houses for provided user
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<HouseSimpleInfo>> GetMembershipHouseInfosAsync(
            decimal userId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete available house by id for provided user
        /// </summary>
        /// <param name="houseId">House id</param>
        /// <param name="userId">User id</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task DeleteHouseByIdAsync(
            BsonObjectId houseId,
            decimal userId,
            CancellationToken cancellationToken = default);
    }
}
