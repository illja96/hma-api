using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using HMA.BLL.Tier1.Exceptions.House;
using HMA.BLL.Tier1.Exceptions.Invite;
using HMA.BLL.Tier1.Exceptions.User;
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

        public async Task<List<HouseInviteInfo>> GetInvitesByUserIdAsync(
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

            var houseFilter = Builders<HouseInfo>.Filter
                .Eq(hi => hi.Id, houseInviteCreationRequest.HouseId);
            var houseCount = await _houseRepository.CountAsync(houseFilter, cancellationToken);
            if (houseCount != 1)
            {
                throw new HouseNotFoundException();
            }

            var invitedByUserFilter = Builders<UserInfo>.Filter
                .Eq(ui => ui.GoogleId, houseInviteCreationRequest.InvitedByUserId);
            var invitedByUserFilterCount = await _userRepository.CountAsync(invitedByUserFilter, cancellationToken);
            if (invitedByUserFilterCount != 1)
            {
                throw new UserNotFoundException();
            }

            var houseInvite = _mapper.Map<HouseInviteInfo>(houseInviteCreationRequest);
            houseInvite.CreatedAt = DateTime.UtcNow;

            var userFilter = Builders<UserInfo>.Filter
                .Eq(ui => ui.Email, houseInviteCreationRequest.UserEmail);

            try
            {
                var user = await _userRepository.FindOneAsync(userFilter, cancellationToken);

                houseInvite.UserId = user.GoogleId;
                houseInvite.UserEmail = user.Email;
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

        public async Task AcceptInviteAsync(
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
                .Push(hi => hi.MemberIds, houseInvite.UserId);

            await _houseRepository.UpdateOneAsync(
                houseFilter,
                houseUpdate,
                cancellationToken);
        }

        public async Task DeclineInviteAsync(
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
