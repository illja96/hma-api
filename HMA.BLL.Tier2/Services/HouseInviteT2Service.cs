using HMA.BLL.Tier2.Services.Interfaces;
using HMA.DTO.Models.Invite;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HMA.BLL.Tier1.Services.Interfaces;

namespace HMA.BLL.Tier2.Services
{
    public class HouseInviteT2Service : IHouseInviteT2Service
    {
        private readonly IHouseInviteT1Service _houseInviteService;

        public HouseInviteT2Service(IHouseInviteT1Service houseInviteService)
        {
            _houseInviteService = houseInviteService;
        }

        public async Task<List<HouseInviteInfo>> GetInvitesByUserIdAsync(
            decimal userId,
            CancellationToken cancellationToken = default)
        {
            var houseInvites = await _houseInviteService.GetInvitesByUserIdAsync(
                userId,
                cancellationToken);

            return houseInvites;
        }

        public async Task<HouseInviteInfo> CreateInviteAsync(
            HouseInviteCreationRequest houseInviteCreationRequest,
            CancellationToken cancellationToken = default)
        {
            var houseInvite = await _houseInviteService.CreateInviteAsync(
                houseInviteCreationRequest,
                cancellationToken);

            return houseInvite;
        }

        public async Task AcceptInviteAsync(
            decimal userId,
            BsonObjectId inviteId,
            CancellationToken cancellationToken = default)
        {
            await _houseInviteService.AcceptInviteAsync(
                userId,
                inviteId,
                cancellationToken);
        }

        public async Task DeclineInviteAsync(
            decimal userId,
            BsonObjectId inviteId,
            CancellationToken cancellationToken = default)
        {
            await _houseInviteService.DeclineInviteAsync(
                userId,
                inviteId,
                cancellationToken);
        }
    }
}
