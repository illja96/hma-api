using HMA.BLL.Tier2.Services.Interfaces;
using HMA.DTO.Models.Invite;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using HMA.BLL.Tier1.Services.Interfaces;

namespace HMA.BLL.Tier2.Services
{
    public class HouseInviteT2Service : IHouseInviteT2Service
    {
        private readonly IMapper _mapper;

        private readonly IHouseInviteT1Service _houseInviteService;
        private readonly IUserT1Service _userService;
        private readonly IHouseT1Service _houseService;

        public HouseInviteT2Service(
            IMapper mapper,
            IHouseInviteT1Service houseInviteService,
            IUserT1Service userService,
            IHouseT1Service houseService)
        {
            _mapper = mapper;

            _houseInviteService = houseInviteService;
            _userService = userService;
            _houseService = houseService;
        }

        public async Task<List<HouseInviteSimpleInfo>> GetInvitesAvailableForUserAsync(
            decimal userId,
            CancellationToken cancellationToken = default)
        {
            var houseInviteInfos = await _houseInviteService.GetInvitesAvailableForUserAsync(
                userId,
                cancellationToken);

            var userIds = houseInviteInfos
                .Select(hii => hii.InvitedByUserId)
                .Distinct()
                .ToList();

            var houseIds = houseInviteInfos
                .Select(hii => hii.HouseId)
                .Distinct()
                .ToList();

            var userSimpleInfos = await _userService.GetByIdsAsync(userIds, cancellationToken);

            var houseSimpleInfos = await _houseService.GetSimpleHouseInfosByIdsAsync(houseIds, cancellationToken);

            var houseInviteSimpleInfos = _mapper.Map<List<HouseInviteSimpleInfo>>(houseInviteInfos);
            houseInviteSimpleInfos.ForEach(hisi =>
            {
                var invitedByUser = userSimpleInfos.First(usi => usi.GoogleId == hisi.InvitedByUserId);
                hisi.InvitedBy = invitedByUser;

                var houseSimpleInfo = houseSimpleInfos.First(hsi => hsi.Id == hisi.HouseId);
                hisi.HouseInfo = houseSimpleInfo;
            });

            return houseInviteSimpleInfos;
        }

        public async Task<HouseInviteSimpleInfo> CreateInviteAsync(
            HouseInviteCreationRequest houseInviteCreationRequest,
            CancellationToken cancellationToken = default)
        {
            var houseInviteInfo = await _houseInviteService.CreateInviteAsync(
                houseInviteCreationRequest,
                cancellationToken);

            var houseInviteSimpleInfo = _mapper.Map<HouseInviteSimpleInfo>(houseInviteInfo);

            return houseInviteSimpleInfo;
        }

        public async Task AcceptInviteAvailableForUserAsync(
            decimal userId,
            BsonObjectId inviteId,
            CancellationToken cancellationToken = default)
        {
            await _houseInviteService.AcceptInviteAvailableForUserAsync(
                userId,
                inviteId,
                cancellationToken);
        }

        public async Task DeclineInviteAvailableForUserAsync(
            decimal userId,
            BsonObjectId inviteId,
            CancellationToken cancellationToken = default)
        {
            await _houseInviteService.DeclineInviteAvailableForUserAsync(
                userId,
                inviteId,
                cancellationToken);
        }
    }
}
