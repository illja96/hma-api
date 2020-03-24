using System.Threading;
using System.Threading.Tasks;
using HMA.DTO.ViewModels.House;
using Microsoft.AspNetCore.Mvc;

namespace HMA.BLL.Wrappers.Wrappers.Interfaces
{
    public interface IWrappedHouseService
    {
        /// <summary>
        /// Get houses available to current user
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ObjectResult> GetHousesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Get house by id.
        /// Returns house only if house is available to current user
        /// </summary>
        /// <param name="houseId">House id</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ObjectResult> GetHouseByIdAsync(
            string houseId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Create new house
        /// </summary>
        /// <param name="houseCreationRequestViewModel">House creation request</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ObjectResult> CreateHouseAsync(
            HouseCreationRequestViewModel houseCreationRequestViewModel,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete house by id.
        /// Deletes house only of current user is owner of a house
        /// </summary>
        /// <param name="houseId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ObjectResult> DeleteHouseByIdAsync(
            string houseId,
            CancellationToken cancellationToken = default);
    }
}
