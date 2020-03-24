using System.Threading;
using System.Threading.Tasks;
using HMA.BLL.Wrappers.Wrappers.Interfaces;
using HMA.DTO.ViewModels.House;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HMA.API.Controllers
{
    [Authorize]
    [ApiController]
    public class HouseController : ControllerBase
    {
        private readonly IWrappedHouseService _wrappedHouseService;

        public HouseController(IWrappedHouseService wrappedHouseService)
        {
            _wrappedHouseService = wrappedHouseService;
        }

        /// <summary>
        /// Get all houses
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("houses")]
        [ProducesResponseType(typeof(HouseInfosViewModel), 200)]
        public async Task<IActionResult> GetHousesAsync(CancellationToken cancellationToken = default)
        {
            var result = await _wrappedHouseService.GetHousesAsync(cancellationToken);
            return result;
        }

        /// <summary>
        /// Get house by id
        /// </summary>
        /// <param name="houseId">House id</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("houses/{houseId}")]
        [ProducesResponseType(typeof(HouseInfoViewModel), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetHouseByIdAsync(
            [FromRoute] string houseId,
            CancellationToken cancellationToken = default)
        {
            var result = await _wrappedHouseService.GetHouseByIdAsync(houseId, cancellationToken);
            return result;
        }

        /// <summary>
        /// Create new house
        /// </summary>
        /// <param name="houseCreationRequestViewModel">House creation request</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("houses")]
        [ProducesResponseType(typeof(HouseInfoViewModel), 200)]
        public async Task<IActionResult> CreateHouseAsync(
            [FromBody] HouseCreationRequestViewModel houseCreationRequestViewModel,
            CancellationToken cancellationToken = default)
        {
            var result = await _wrappedHouseService.CreateHouseAsync(houseCreationRequestViewModel, cancellationToken);
            return result;
        }

        /// <summary>
        /// Delete house by id
        /// </summary>
        /// <param name="houseId">House id</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete("houses/{houseId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteHouseByIdAsync(
            [FromRoute] string houseId,
            CancellationToken cancellationToken = default)
        {
            var result = await _wrappedHouseService.DeleteHouseByIdAsync(houseId, cancellationToken);
            return result;
        }
    }
}
