using HMA.DTO.ViewModels.Invite;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace HMA.BLL.Tier3.Services.Interfaces
{
    public interface IHouseInviteT3Service
    {
        /// <summary>
        /// Get houses invites available for current user
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        Task<ObjectResult> GetInvitesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Create house invite
        /// </summary>
        /// <param name="houseInviteCreationRequestViewModel">House invite creation request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task<ObjectResult> CreateInviteAsync(
            HouseInviteCreationRequestViewModel houseInviteCreationRequestViewModel,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Accept house invite available for current user
        /// </summary>
        /// <param name="inviteId"></param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task<ObjectResult> AcceptInviteAsync(
            string inviteId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Decline house invite available for current user
        /// </summary>
        /// <param name="inviteId"></param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task<ObjectResult> DeclineInviteAsync(
            string inviteId,
            CancellationToken cancellationToken = default);
    }
}
