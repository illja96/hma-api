using HMA.BLL.Tier3.Services.Interfaces;
using HMA.DTO.ViewModels.Invite;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HMA.API.Controllers
{
    [ApiController]
    [Authorize]
    public class HouseInviteController : ControllerBase
    {
        private readonly IHouseInviteT3Service _houseInviteService;

        public HouseInviteController(IHouseInviteT3Service houseInviteService)
        {
            _houseInviteService = houseInviteService;
        }

        /// <summary>
        /// Get house invites
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        [HttpGet("houses/invites")]
        [ProducesResponseType(typeof(List<HouseInviteSimpleInfoViewModel>), 200)]
        public async Task<IActionResult> GetInvitesAsync(
            CancellationToken cancellationToken = default)
        {
            var result = await _houseInviteService.GetInvitesAsync(cancellationToken);
            return result;
        }

        /// <summary>
        /// Create house invite
        /// </summary>
        /// <param name="houseInviteCreationRequestViewModel">House invite request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        [HttpPost("houses/invites")]
        [ProducesResponseType(typeof(HouseInviteSimpleInfoViewModel), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> CreateInviteAsync(
            [FromBody] HouseInviteCreationRequestViewModel houseInviteCreationRequestViewModel,
            CancellationToken cancellationToken = default)
        {
            var result = await _houseInviteService.CreateInviteAsync(
                houseInviteCreationRequestViewModel,
                cancellationToken);

            return result;
        }

        /// <summary>
        /// Accept invite
        /// </summary>
        /// <param name="inviteId">Invite id</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        [HttpPost("houses/invites/{inviteId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> AcceptInviteAsync(
            [FromRoute] string inviteId,
            CancellationToken cancellationToken = default)
        {
            var result = await _houseInviteService.AcceptInviteAsync(
                inviteId,
                cancellationToken);

            return result;
        }

        /// <summary>
        /// Decline invite
        /// </summary>
        /// <param name="inviteId">Invite id</param>
        /// <param name="cancellationToken">Cancellation token</param>
        [HttpDelete("houses/invites/{inviteId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeclineInviteAsync(
            [FromRoute] string inviteId,
            CancellationToken cancellationToken = default)
        {
            var result = await _houseInviteService.DeclineInviteAsync(
                inviteId,
                cancellationToken);

            return result;
        }
    }
}
