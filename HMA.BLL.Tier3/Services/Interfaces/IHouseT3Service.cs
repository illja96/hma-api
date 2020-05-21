using System.Threading;
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
        /// Get houses available to current user
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
        /// Delete house by id if user is an owner.
        /// Leave house by id if user is a member
        /// </summary>
        /// <param name="houseId">House id</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task<ObjectResult> DeleteOrLeaveHouseByIdAsync(
            string houseId,
            CancellationToken cancellationToken = default);
    }
}
