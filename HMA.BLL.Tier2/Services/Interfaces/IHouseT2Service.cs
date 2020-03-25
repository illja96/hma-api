using System.Threading;
using System.Threading.Tasks;
using HMA.DTO.Models.House;
using MongoDB.Bson;

namespace HMA.BLL.Tier2.Services.Interfaces
{
    public interface IHouseT2Service
    {
        /// <summary>
        /// Get available houses for provided user
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<AvailableHousesInfo> GetAvailableHousesForUserAsync(
            decimal userId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get available house by id for provided user
        /// </summary>
        /// <param name="houseId">House id</param>
        /// <param name="userId">User id</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<HouseSimpleInfo> GetHouseByIdAsync(
            BsonObjectId houseId,
            decimal userId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Create a new house
        /// </summary>
        /// <param name="houseCreationRequest">House creation request</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<HouseSimpleInfo> CreateHouseAsync(
            HouseCreationRequest houseCreationRequest,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete available house for provided user
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
