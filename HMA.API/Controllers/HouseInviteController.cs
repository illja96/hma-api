using HMA.DTO.ViewModels.Invite;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace HMA.API.Controllers
{
    [ApiController]
    [Authorize]
    public class HouseInviteController : ControllerBase
    {
        /// <summary>
        /// Get house invites
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        [HttpGet("houses/invites")]
        public async Task<IActionResult> GetInvitesAsync(
            CancellationToken cancellationToken = default)
        {
            return new OkResult();
        }

        /// <summary>
        /// Create house invite
        /// </summary>
        /// <param name="houseInviteCreationRequestViewModel">House invite request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        [HttpPost("houses/invites")]
        public async Task<IActionResult> CreateInviteAsync(
            [FromBody] HouseInviteCreationRequestViewModel houseInviteCreationRequestViewModel,
            CancellationToken cancellationToken = default)
        {
            return new OkResult();
        }

        /// <summary>
        /// Accept invite
        /// </summary>
        /// <param name="inviteId">Invite id</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        [HttpPost("houses/invites/{inviteId}")]
        public async Task<IActionResult> AcceptInviteAsync(
            [FromRoute] string inviteId,
            CancellationToken cancellationToken = default)
        {
            return new OkResult();
        }

        /// <summary>
        /// Decline invite
        /// </summary>
        /// <param name="inviteId">Invite id</param>
        /// <param name="cancellationToken">Cancellation token</param>
        [HttpDelete("houses/invites/{inviteId}")]
        public async Task<IActionResult> DeclineInviteAsync(
            [FromRoute] string inviteId,
            CancellationToken cancellationToken = default)
        {
            return new OkResult();
        }
    }
}
