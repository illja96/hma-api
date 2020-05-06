using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using HMA.BLL.Tier1.Exceptions.House;
using HMA.BLL.Tier1.Services.Interfaces;
using HMA.DAL.Factories;
using HMA.DAL.Repositories.Interfaces;
using HMA.DTO.Models.House;
using HMA.DTO.Models.Transactions;
using MongoDB.Bson;
using MongoDB.Driver;

namespace HMA.BLL.Tier1.Services
{
    public class TransactionT1Service : ITransactionT1Service
    {
        private readonly IMapper _mapper;

        private readonly IGenericRepository<HouseInfo> _houseRepository;

        public TransactionT1Service(
            IMapper mapper,
            IGenericRepository<HouseInfo> houseRepository)
        {
            _mapper = mapper;

            _houseRepository = houseRepository;
        }

        public async Task<List<string>> GetUsedTagsByHouseIdAsync(
            BsonObjectId houseId,
            decimal userId,
            CancellationToken cancellationToken = default)
        {
            var houseFilter = Builders<HouseInfo>.Filter.Eq(hi => hi.Id, houseId);
            var userFilter = BuilderFactory.CreateHouseOwnerOrMembershipFilter(userId);

            var project = Builders<HouseInfo>.Projection.Expression(hi => hi.Transactions.SelectMany(t => t.Tags).Distinct().ToList());

            var pipeline = new EmptyPipelineDefinition<HouseInfo>()
                .Match(houseFilter)
                .Match(userFilter)
                .Project(project);

            try
            {
                var tags = await _houseRepository.FindOneAsync(pipeline, cancellationToken);
                return tags;
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

        public async Task<List<TransactionInfo>> GetTransactionsByHouseIdAsync(
            BsonObjectId houseId,
            decimal userId,
            CancellationToken cancellationToken = default)
        {
            var houseFilter = Builders<HouseInfo>.Filter.Eq(hi => hi.Id, houseId);
            var userFilter = BuilderFactory.CreateHouseOwnerOrMembershipFilter(userId);

            var project = Builders<HouseInfo>.Projection.Expression(hi => hi.Transactions);

            var pipeline = new EmptyPipelineDefinition<HouseInfo>()
                .Match(houseFilter)
                .Match(userFilter)
                .Project(project);

            try
            {
                var transactionInfos = await _houseRepository.FindOneAsync(pipeline, cancellationToken);
                return transactionInfos;
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

        public async Task<TransactionInfo> CreateTransactionAsync(
            TransactionCreationRequest transactionCreationRequest,
            decimal userId,
            CancellationToken cancellationToken = default)
        {
            var transaction = _mapper.Map<TransactionInfo>(transactionCreationRequest);
            transaction.Id = ObjectId.GenerateNewId();
            transaction.CreationDate = DateTime.UtcNow;

            var houseFilter = Builders<HouseInfo>.Filter.Eq(hi => hi.Id, transactionCreationRequest.HouseId);
            var userFilter = BuilderFactory.CreateHouseOwnerOrMembershipFilter(userId);
            var filter = Builders<HouseInfo>.Filter.And(houseFilter, userFilter);

            var update = Builders<HouseInfo>.Update.Push(hi => hi.Transactions, transaction);

            await _houseRepository.UpdateOneAsync(
                filter,
                update,
                cancellationToken);

            return transaction;
        }

        public async Task DeleteTransactionAsync(
            BsonObjectId houseId,
            BsonObjectId transactionId,
            decimal userId,
            CancellationToken cancellationToken = default)
        {
            var houseFilter = Builders<HouseInfo>.Filter.Eq(hi => hi.Id, houseId);
            var userFilter = BuilderFactory.CreateHouseOwnerOrMembershipFilter(userId);
            var filter = Builders<HouseInfo>.Filter.And(houseFilter, userFilter);

            var update = Builders<HouseInfo>.Update.PullFilter(
                hi => hi.Transactions,
                Builders<TransactionInfo>.Filter.Eq(t => t.Id, transactionId));

            await _houseRepository.UpdateOneAsync(
                filter,
                update,
                cancellationToken);
        }
    }
}
