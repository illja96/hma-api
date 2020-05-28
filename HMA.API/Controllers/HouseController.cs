using System;
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
        /// <param name="cancellationToken">Cancellation token</param>
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
        /// <param name="cancellationToken">Cancellation token</param>
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
        /// <param name="cancellationToken">Cancellation token</param>
        [HttpPost("houses")]
        [ProducesResponseType(typeof(HouseSimpleInfoViewModel), 200)]
        public async Task<IActionResult> CreateHouseAsync(
            [FromBody] HouseCreationRequestViewModel houseCreationRequestViewModel,
            CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                var badResult = new BadRequestObjectResult(ModelState);
                return badResult;
            }

            var result = await _houseService.CreateHouseAsync(houseCreationRequestViewModel, cancellationToken);
            return result;
        }

        /// <summary>
        /// Delete house by id if user is an owner.
        /// Leave a house by id if user is a member
        /// </summary>
        /// <param name="houseId">House id</param>
        /// <param name="cancellationToken">Cancellation token</param>
        [HttpDelete("houses/{houseId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteOrLeaveHouseByIdAsync(
            [FromRoute] string houseId,
            CancellationToken cancellationToken = default)
        {
            var result = await _houseService.DeleteOrLeaveHouseByIdAsync(houseId, cancellationToken);
            return result;
        }

        /// <summary>
        /// Remove house member by id
        /// </summary>
        /// <param name="houseId">House id</param>
        /// <param name="memberId">Member id</param>
        /// <param name="cancellationToken">Cancellation token</param>
        [HttpDelete("houses/{houseId}/members/{memberId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> RemoveHouseMemberByIdAsync(
            [FromRoute] string houseId,
            [FromRoute] decimal memberId,
            CancellationToken cancellationToken = default)
        {
            var result = await _houseService.RemoveHouseMemberByIdAsync(houseId, memberId, cancellationToken);
            return result;
        }
    }
}
