﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using HMA.BLL.Tier1.Exceptions.House;
using HMA.BLL.Tier1.Services.Interfaces;
using HMA.DAL.Factories;
using HMA.DAL.Repositories.Interfaces;
using HMA.DTO.Models.House;
using MongoDB.Bson;
using MongoDB.Driver;

namespace HMA.BLL.Tier1.Services
{
    public class HouseT1Service : IHouseT1Service
    {
        private readonly IMapper _mapper;

        private readonly IGenericRepository<HouseInfo> _houseRepository;

        public HouseT1Service(
            IMapper mapper,
            IGenericRepository<HouseInfo> houseRepository)
        {
            _mapper = mapper;
            _houseRepository = houseRepository;
        }

        public async Task<HouseInfo> GetHouseInfoByIdAvailableForUserAsync(
            BsonObjectId houseId,
            decimal userId,
            CancellationToken cancellationToken = default)
        {
            var houseFilter = Builders<HouseInfo>.Filter.Eq(hi => hi.Id, houseId);
            var userFilter = BuilderFactory.CreateHouseOwnerOrMembershipFilter(userId);

            var pipeline = new EmptyPipelineDefinition<HouseInfo>()
                .Match(houseFilter)
                .Match(userFilter);

            try
            {
                var houseInfo = await _houseRepository.FindOneAsync(pipeline, cancellationToken);
                return houseInfo;
            }
            catch (InvalidOperationException e)
            {
                if (e.Message.Equals("Sequence contains no elements", StringComparison.Ordinal))
                {
                    throw new HouseNotFoundException();
                }

                throw;
            }
        }

        public async Task<HouseSimpleInfo> GetSimpleHouseInfoByIdAvailableForUserAsync(
            BsonObjectId houseId,
            decimal userId,
            CancellationToken cancellationToken = default)
        {
            var houseFilter = Builders<HouseInfo>.Filter.Eq(hi => hi.Id, houseId);
            var userFilter = BuilderFactory.CreateHouseOwnerOrMembershipFilter(userId);

            var projection = Builders<HouseInfo>.Projection.Expression(hi => new HouseSimpleInfo()
            {
                Id = hi.Id,
                OwnerId = hi.OwnerId,
                Name = hi.Name,
                MemberIds = hi.MemberIds,
                CreationDate = hi.CreationDate
            });

            var pipeline = new EmptyPipelineDefinition<HouseInfo>()
                .Match(houseFilter)
                .Match(userFilter)
                .Project(projection);

            try
            {
                var houseSimpleInfo = await _houseRepository.FindOneAsync(pipeline, cancellationToken);
                return houseSimpleInfo;
            }
            catch (InvalidOperationException e)
            {
                if (e.Message.Equals("Sequence contains no elements", StringComparison.Ordinal))
                {
                    throw new HouseNotFoundException();
                }

                throw;
            }
        }

        public async Task<List<HouseSimpleInfo>> GetSimpleHouseInfosByIdsAsync(
            List<BsonObjectId> houseIds,
            CancellationToken cancellationToken = default)
        {
            var filter = Builders<HouseInfo>.Filter.In(hi => hi.Id, houseIds);
            var projection = Builders<HouseInfo>.Projection.Expression(hi => new HouseSimpleInfo()
            {
                Id = hi.Id,
                OwnerId = hi.OwnerId,
                Name = hi.Name,
                MemberIds = hi.MemberIds,
                CreationDate = hi.CreationDate
            });

            var pipeline = new EmptyPipelineDefinition<HouseInfo>()
                .Match(filter)
                .Project(projection);

            var houseSimpleInfos = await _houseRepository.FindAsync(pipeline, cancellationToken);

            return houseSimpleInfos;
        }

        public async Task<List<HouseSimpleInfo>> GetOwnedHouseSimpleInfosOfUserAsync(
            decimal ownerId,
            CancellationToken cancellationToken = default)
        {
            var filter = Builders<HouseInfo>.Filter.Eq(hi => hi.OwnerId, ownerId);
            var sort = Builders<HouseInfo>.Sort.Descending(hi => hi.Name);
            var projection = Builders<HouseInfo>.Projection.Expression(hi => new HouseSimpleInfo()
            {
                Id = hi.Id,
                OwnerId = hi.OwnerId,
                Name = hi.Name,
                MemberIds = hi.MemberIds,
                CreationDate = hi.CreationDate
            });

            var pipeline = new EmptyPipelineDefinition<HouseInfo>()
                .Match(filter)
                .Sort(sort)
                .Project(projection);

            var houseSimpleInfos = await _houseRepository.FindAsync(pipeline, cancellationToken);

            return houseSimpleInfos;
        }

        public async Task<List<HouseSimpleInfo>> GetMembershipHouseSimpleInfosOfUserAsync(
            decimal userId,
            CancellationToken cancellationToken = default)
        {
            var filter = Builders<HouseInfo>.Filter.All(hi => hi.MemberIds, new[] { userId });
            var sort = Builders<HouseInfo>.Sort
                .Descending(hi => hi.OwnerId)
                .Descending(hi => hi.Name);
            var projection = Builders<HouseInfo>.Projection.Expression(hi => new HouseSimpleInfo()
            {
                Id = hi.Id,
                OwnerId = hi.OwnerId,
                Name = hi.Name,
                MemberIds = hi.MemberIds,
                CreationDate = hi.CreationDate
            });

            var pipeline = new EmptyPipelineDefinition<HouseInfo>()
                .Match(filter)
                .Sort(sort)
                .Project(projection);

            var houseSimpleInfos = await _houseRepository.FindAsync(pipeline, cancellationToken);

            return houseSimpleInfos;
        }

        public async Task<HouseInfo> CreateHouseAsync(
            HouseCreationRequest houseCreationRequest,
            CancellationToken cancellationToken = default)
        {
            var houseInfo = _mapper.Map<HouseInfo>(houseCreationRequest);
            houseInfo.CreationDate = DateTime.UtcNow;

            houseInfo = await _houseRepository.InsertAsync(houseInfo, cancellationToken);

            return houseInfo;
        }

        public async Task DeleteOrLeaveHouseByIdAvailableForUserAsync(
            BsonObjectId houseId,
            decimal userId,
            CancellationToken cancellationToken = default)
        {
            var houseFilter = Builders<HouseInfo>.Filter.Eq(hi => hi.Id, houseId);
            var ownerFilter = Builders<HouseInfo>.Filter.Eq(hi => hi.OwnerId, userId);
            var memberFilter = Builders<HouseInfo>.Filter.All(hi => hi.MemberIds, new[] { userId });

            var filter = Builders<HouseInfo>.Filter.And(houseFilter, ownerFilter);

            var deletedCount = await _houseRepository.DeleteAsync(filter, cancellationToken);
            if (deletedCount == 1)
            {
                return;
            }

            filter = Builders<HouseInfo>.Filter.And(houseFilter, memberFilter);

            var update = Builders<HouseInfo>.Update.Pull(hi => hi.MemberIds, userId);

            var updatedHouseInfo = await _houseRepository.UpdateOneAsync(filter, update, cancellationToken);
            if (updatedHouseInfo == null)
            {
                throw new HouseNotFoundException();
            }
        }

        public async Task RemoveHouseMemberByIdAvailableForUserAsync(
            BsonObjectId houseId,
            decimal userId,
            decimal memberId,
            CancellationToken cancellationToken = default)
        {
            var houseFilter = Builders<HouseInfo>.Filter.Eq(hi => hi.Id, houseId);
            var ownerFilter = Builders<HouseInfo>.Filter.Eq(hi => hi.OwnerId, userId);

            var filter = Builders<HouseInfo>.Filter.And(houseFilter, ownerFilter);

            var update = Builders<HouseInfo>.Update.Pull(hi => hi.MemberIds, memberId);

            var updatedHouseInfo = await _houseRepository.UpdateOneAsync(filter, update, cancellationToken);
            if (updatedHouseInfo == null)
            {
                throw new HouseNotFoundException();
            }
        }
    }
}
