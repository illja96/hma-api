using System.Threading;
using System.Threading.Tasks;
using HMA.BLL.Tier3.Services.Interfaces;
using HMA.DTO.ViewModels.House;
using HMA.Infrastructure.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HMA.API.Controllers
{
    [ApiController]
    [Authorize(PolicyConstants.UserRegistered)]
    public class HouseController : ControllerBase
    {
        private readonly IHouseT3Service _houseService;

        public HouseController(IHouseT3Service houseService)
        {
            _houseService = houseService;
        }

        /// <summary>
        /// Get all houses
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("houses")]
        [ProducesResponseType(typeof(AvailableHousesInfoViewModel), 200)]
        public async Task<IActionResult> GetHousesAsync(CancellationToken cancellationToken = default)
        {
            var result = await _houseService.GetHousesAsync(cancellationToken);
            return result;
        }

        /// <summary>
        /// Get house by id
        /// </summary>
        /// <param name="houseId">House id</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("houses/{houseId}")]
        [ProducesResponseType(typeof(HouseSimpleInfoViewModel), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetHouseByIdAsync(
            [FromRoute] string houseId,
            CancellationToken cancellationToken = default)
        {
            var result = await _houseService.GetHouseByIdAsync(houseId, cancellationToken);
            return result;
        }

        /// <summary>
        /// Create new house
        /// </summary>
        /// <param name="houseCreationRequestViewModel">House creation request</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("houses")]
        [ProducesResponseType(typeof(HouseSimpleInfoViewModel), 200)]
        public async Task<IActionResult> CreateHouseAsync(
            [FromBody] HouseCreationRequestViewModel houseCreationRequestViewModel,
            CancellationToken cancellationToken = default)
        {
            var result = await _houseService.CreateHouseAsync(houseCreationRequestViewModel, cancellationToken);
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
            var result = await _houseService.DeleteHouseByIdAsync(houseId, cancellationToken);
            return result;
        }
    }
}
