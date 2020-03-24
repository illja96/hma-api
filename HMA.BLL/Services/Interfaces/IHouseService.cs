using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HMA.DTO.Models.House;
using MongoDB.Bson;

namespace HMA.BLL.Services.Interfaces
{
    /// <summary>
    /// House server
    /// </summary>
    public interface IHouseService
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
        /// Get house by id.
        /// Returns house only if user is owner or member of a house
        /// </summary>
        /// <param name="houseId">House id</param>
        /// <param name="userId">User id</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<HouseInfo> GetHouseByIdAsync(
            BsonObjectId houseId,
            decimal userId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get owned houses
        /// </summary>
        /// <param name="ownerId">Owner (user) id</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<HouseSimpleInfo>> GetOwnedHousesAsync(
            decimal ownerId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get member of houses
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<HouseSimpleInfo>> GetMemberOfHousesAsync(
            decimal userId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete house by id.
        /// Deletes house only if user is owner of a house
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
