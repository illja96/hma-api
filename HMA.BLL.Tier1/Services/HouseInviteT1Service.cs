using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using HMA.BLL.Tier1.Exceptions.House;
using HMA.BLL.Tier1.Exceptions.Invite;
using HMA.BLL.Tier1.Options;
using HMA.BLL.Tier1.Services.Interfaces;
using HMA.DAL.Repositories.Interfaces;
using HMA.DTO.Models.House;
using HMA.DTO.Models.Invite;
using HMA.DTO.Models.User;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace HMA.BLL.Tier1.Services
{
    public class HouseInviteT1Service : IHouseInviteT1Service
    {
        private readonly IMapper _mapper;
        private readonly IOptions<HouseInviteOptions> _houseInviteOptions;

        private readonly IGenericRepository<HouseInviteInfo> _houseInviteRepository;
        private readonly IGenericRepository<HouseInfo> _houseRepository;
        private readonly IGenericRepository<UserInfo> _userRepository;

        public HouseInviteT1Service(
            IMapper mapper,
            IOptions<HouseInviteOptions> houseInviteOptions,
            IGenericRepository<HouseInviteInfo> houseInviteRepository,
            IGenericRepository<HouseInfo> houseRepository,
            IGenericRepository<UserInfo> userRepository)
        {
            _mapper = mapper;
            _houseInviteOptions = houseInviteOptions;

            _houseInviteRepository = houseInviteRepository;
            _houseRepository = houseRepository;
            _userRepository = userRepository;
        }

        public async Task<List<HouseInviteInfo>> GetInvitesAvailableForUserAsync(
            decimal userId,
            CancellationToken cancellationToken = default)
        {
            var filter = Builders<HouseInviteInfo>.Filter
                .Eq(hii => hii.UserId, userId);

            var houseInvites = await _houseInviteRepository.FindAsync(filter, cancellationToken);
            return houseInvites;
        }

        public async Task<HouseInviteInfo> CreateInviteAsync(
            HouseInviteCreationRequest houseInviteCreationRequest,
            CancellationToken cancellationToken = default)
        {
            var houseInvitesFilter = Builders<HouseInviteInfo>.Filter
                .Eq(hii => hii.InvitedByUserId, houseInviteCreationRequest.InvitedByUserId);

            var houseInvitesCount = await _houseInviteRepository.CountAsync(
                houseInvitesFilter,
                cancellationToken);

            if (houseInvitesCount >= _houseInviteOptions.Value.MaxInvitesPerUser)
            {
                throw new TooManyHouseInvitesException();
            }

            var houseIdFilter = Builders<HouseInfo>.Filter
                .Eq(hi => hi.Id, houseInviteCreationRequest.HouseId);
            var houseOwnerFilter = Builders<HouseInfo>.Filter
                .Eq(hi => hi.OwnerId, houseInviteCreationRequest.InvitedByUserId);
            var houseFilter = Builders<HouseInfo>.Filter
                .And(houseIdFilter, houseOwnerFilter);

            var houseCount = await _houseRepository.CountAsync(houseFilter, cancellationToken);
            if (houseCount != 1)
            {
                throw new HouseNotFoundException();
            }

            var houseInvite = _mapper.Map<HouseInviteInfo>(houseInviteCreationRequest);
            houseInvite.CreatedAt = DateTime.UtcNow;

            var userFilter = Builders<UserInfo>.Filter
                .Regex(ui => ui.Email, new BsonRegularExpression($"^{houseInviteCreationRequest.UserEmail}$", "i"));

            try
            {
                var userInfo = await _userRepository.FindOneAsync(userFilter, cancellationToken);

                var isUserInvitingHimself = userInfo.GoogleId == houseInvite.InvitedByUserId;
                if (isUserInvitingHimself)
                {
                    throw new SelfReferencingHouseInviteException();
                }

                houseInvite.UserId = userInfo.GoogleId;
                houseInvite.UserEmail = userInfo.Email;
            }
            catch (InvalidOperationException e)
            {
                if (e.Message.Equals("Sequence contains no elements", StringComparison.Ordinal))
                {
                    throw new HouseNotFoundException();
                }

                throw;
            }

            houseInvite = await _houseInviteRepository.InsertAsync(houseInvite, cancellationToken);

            return houseInvite;
        }

        public async Task AcceptInviteAvailableForUserAsync(
            decimal userId,
            BsonObjectId inviteId,
            CancellationToken cancellationToken = default)
        {
            var houseInviteFilter = Builders<HouseInviteInfo>.Filter
                .Eq(hii => hii.Id, inviteId);

            HouseInviteInfo houseInvite;
            try
            {
                houseInvite = await _houseInviteRepository.FindOneAsync(
                    houseInviteFilter,
                    cancellationToken);
            }
            catch (InvalidOperationException e)
            {
                if (e.Message.Equals("Sequence contains no elements", StringComparison.Ordinal))
                {
                    throw new HouseInviteNotFoundException();
                }

                throw;
            }

            var houseFilter = Builders<HouseInfo>.Filter
                .Eq(hi => hi.Id, houseInvite.HouseId);

            var houseUpdate = Builders<HouseInfo>.Update
                .Push(hi => hi.MemberIds, houseInvite.UserId.Value);

            await _houseRepository.UpdateOneAsync(
                houseFilter,
                houseUpdate,
                cancellationToken);

            await _houseInviteRepository.DeleteAsync(houseInviteFilter, cancellationToken);
        }

        public async Task DeclineInviteAvailableForUserAsync(
            decimal userId,
            BsonObjectId inviteId,
            CancellationToken cancellationToken = default)
        {
            var filter = Builders<HouseInviteInfo>.Filter
                .Eq(hii => hii.Id, inviteId);

            var deletedCount = await _houseInviteRepository.DeleteAsync(
                filter,
                cancellationToken);

            if (deletedCount != 1)
            {
                throw new HouseInviteNotFoundException();
            }
        }
    }
}
