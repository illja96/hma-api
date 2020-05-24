using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using HMA.BLL.Tier1.Exceptions.User;
using HMA.BLL.Tier1.Services.Interfaces;
using HMA.BLL.Tier2.Services.Interfaces;
using HMA.DTO.Models.House;
using HMA.DTO.Models.User;
using MongoDB.Bson;

namespace HMA.BLL.Tier2.Services
{
    public class HouseT2Service : IHouseT2Service
    {
        private readonly IMapper _mapper;

        private readonly IHouseT1Service _houseService;
        private readonly IUserT1Service _userService;

        public HouseT2Service(
            IMapper mapper,
            IHouseT1Service houseService,
            IUserT1Service userService)
        {
            _mapper = mapper;
            _houseService = houseService;
            _userService = userService;
        }

        public async Task<AvailableHousesInfo> GetAvailableHouseInfosForUserAsync(
            decimal userId,
            CancellationToken cancellationToken = default)
        {
            var ownedHouseInfos = await _houseService.GetOwnedHouseSimpleInfosOfUserAsync(
                userId,
                cancellationToken);

            var membershipHouseInfos = await _houseService.GetMembershipHouseSimpleInfosOfUserAsync(
                userId,
                cancellationToken);

            var userIds = new List<decimal>()
                .Concat(ownedHouseInfos.Select(ohi => ohi.OwnerId))
                .Concat(ownedHouseInfos.SelectMany(ohi => ohi.MemberIds))
                .Concat(membershipHouseInfos.Select(mhi => mhi.OwnerId))
                .Concat(membershipHouseInfos.SelectMany(mhi => mhi.MemberIds))
                .Distinct()
                .ToList();

            var userInfos = await _userService.GetByIdsAsync(userIds, cancellationToken);

            ownedHouseInfos
                .Concat(membershipHouseInfos)
                .ToList()
                .ForEach(hi =>
                {
                    var ownerInfo = userInfos.FirstOrDefault(ui => ui.GoogleId == hi.OwnerId);
                    hi.OwnerInfo = ownerInfo;

                    var memberInfos = userInfos
                        .Where(ui => hi.MemberIds.Contains(ui.GoogleId))
                        .ToList();
                    hi.MemberInfos = memberInfos;
                });

            var availableHousesInfo = new AvailableHousesInfo()
            {
                Owned = ownedHouseInfos,
                MemberOf = membershipHouseInfos
            };

            return availableHousesInfo;
        }

        public async Task<HouseSimpleInfo> GetHouseSimpleInfoByIdAvailableForUserAsync(
            BsonObjectId houseId,
            decimal userId,
            CancellationToken cancellationToken = default)
        {
            var houseSimpleInfo = await _houseService.GetSimpleHouseInfoByIdAvailableForUserAsync(houseId, userId, cancellationToken);

            try
            {
                var ownerInfo = await _userService.GetByIdAsync(houseSimpleInfo.OwnerId, cancellationToken);
                var ownerSimpleInfo = _mapper.Map<UserSimpleInfo>(ownerInfo);
                houseSimpleInfo.OwnerInfo = ownerSimpleInfo;
            }
            catch (UserNotFoundException) { }

            var memberInfos = await _userService.GetByIdsAsync(houseSimpleInfo.MemberIds, cancellationToken);
            var memberSimpleInfos = _mapper.Map<List<UserSimpleInfo>>(memberInfos);
            houseSimpleInfo.MemberInfos = memberSimpleInfos;

            return houseSimpleInfo;
        }

        public async Task<HouseSimpleInfo> CreateHouseAsync(
            HouseCreationRequest houseCreationRequest,
            CancellationToken cancellationToken = default)
        {
            var houseInfo = await _houseService.CreateHouseAsync(houseCreationRequest, cancellationToken);
            var houseSimpleInfo = _mapper.Map<HouseSimpleInfo>(houseInfo);

            return houseSimpleInfo;
        }

        public async Task DeleteOrLeaveHouseByIdAvailableForUserAsync(
            BsonObjectId houseId,
            decimal userId,
            CancellationToken cancellationToken = default)
        {
            await _houseService.DeleteOrLeaveHouseByIdAvailableForUserAsync(houseId, userId, cancellationToken);
        }
    }
}
