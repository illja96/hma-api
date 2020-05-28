using System.Threading;
using System.Threading.Tasks;
using HMA.BLL.Tier1.Exceptions.House;
using HMA.DTO.Models.House;
using MongoDB.Bson;

namespace HMA.BLL.Tier2.Services.Interfaces
{
    public interface IHouseT2Service
    {
        /// <summary>
        /// Get available house infos for provided user
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task<AvailableHousesInfo> GetAvailableHouseInfosForUserAsync(
            decimal userId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get house simple info by id available for provided user
        /// </summary>
        /// <param name="houseId">House id</param>
        /// <param name="userId">User id</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <exception cref="HouseNotFoundException"></exception>
        Task<HouseSimpleInfo> GetHouseSimpleInfoByIdAvailableForUserAsync(
            BsonObjectId houseId,
            decimal userId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Create a new house
        /// </summary>
        /// <param name="houseCreationRequest">House creation request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task<HouseSimpleInfo> CreateHouseAsync(
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
