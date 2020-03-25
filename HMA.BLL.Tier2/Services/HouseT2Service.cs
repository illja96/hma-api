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

        public async Task<AvailableHousesInfo> GetAvailableHousesForUserAsync(
            decimal userId,
            CancellationToken cancellationToken = default)
        {
            var ownedHouseInfos = await _houseService.GetOwnedHouseInfosAsync(
                userId,
                cancellationToken);

            var ownerUserIds = ownedHouseInfos
                .Select(ohi => ohi.OwnerId)
                .Distinct()
                .ToList();
            var ownerUserInfos = await _userService.GetByIdsAsync(ownerUserIds, cancellationToken);

            ownedHouseInfos.ForEach(ohi =>
            {
                var ownerInfo = ownerUserInfos.FirstOrDefault(oui => oui.GoogleId == ohi.OwnerId);
                ohi.OwnerInfo = ownerInfo;
            });

            var membershipHouseInfos = await _houseService.GetMembershipHouseInfosAsync(
                userId,
                cancellationToken);

            var membershipUserIds = membershipHouseInfos
                .SelectMany(mhi => mhi.MemberIds)
                .Distinct()
                .ToList();
            var membershipUserInfos = await _userService.GetByIdsAsync(membershipUserIds, cancellationToken);

            membershipHouseInfos.ForEach(mhi =>
            {
                var memberInfos = membershipUserInfos
                    .Where(mui => mhi.MemberIds.Contains(mui.GoogleId))
                    .ToList();
                mhi.MemberInfos = memberInfos;
            });

            var availableHousesInfo = new AvailableHousesInfo()
            {
                Owned = ownedHouseInfos,
                MemberOf = membershipHouseInfos
            };

            return availableHousesInfo;
        }

        public async Task<HouseSimpleInfo> GetHouseByIdAsync(
            BsonObjectId houseId,
            decimal userId,
            CancellationToken cancellationToken = default)
        {
            var houseSimpleInfo = await _houseService.GetSimpleHouseByIdAsync(houseId, userId, cancellationToken);

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

        public async Task DeleteHouseByIdAsync(
            BsonObjectId houseId,
            decimal userId,
            CancellationToken cancellationToken = default)
        {
            await _houseService.DeleteHouseByIdAsync(houseId, userId, cancellationToken);
        }
    }
}
