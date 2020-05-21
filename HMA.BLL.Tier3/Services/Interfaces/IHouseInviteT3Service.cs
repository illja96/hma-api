using HMA.DTO.ViewModels.Invite;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace HMA.BLL.Tier3.Services.Interfaces
{
    public interface IHouseInviteT3Service
    {
        Task<ObjectResult> GetInvitesAsync(CancellationToken cancellationToken = default);

        Task<ObjectResult> CreateInviteAsync(
            HouseInviteCreationRequestViewModel houseInviteCreationRequestViewModel,
            CancellationToken cancellationToken = default);

        Task<ObjectResult> AcceptInviteAsync(
            string inviteId,
            CancellationToken cancellationToken = default);

        Task<ObjectResult> DeclineInviteAsync(
            string inviteId,
            CancellationToken cancellationToken = default);
    }
}
