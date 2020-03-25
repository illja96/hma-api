using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HMA.DAL.Options;
using HMA.DAL.Repositories.Interfaces;
using HMA.DTO.Models.Base;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace HMA.DAL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T>
        where T : BaseDalModel
    {
        private readonly ILogger _logger;
        private readonly IMongoCollection<T> _mongoDbCollection;

        public GenericRepository(
            ILogger<GenericRepository<T>> logger,
            IMongoClient mongoDbClient,
            IOptions<MongoDbOptions> mongoDbOptions)
        {
            _logger = logger;

            var mongoDbDatabase = mongoDbClient.GetDatabase(mongoDbOptions.Value.DatabaseName);

            var collectionName = GetCollectionName(typeof(T));
            _mongoDbCollection = mongoDbDatabase.GetCollection<T>(collectionName);
        }

        public async Task<List<T>> FindAsync(
            FilterDefinition<T> filterDefinition,
            CancellationToken cancellationToken = default)
        {
            var batches = await _mongoDbCollection.FindAsync(filterDefinition, null, cancellationToken);
            var items = await batches.ToListAsync(cancellationToken);

            return items;
        }

        public async Task<List<TOut>> FindAsync<TOut>(
            PipelineDefinition<T, TOut> pipelineDefinition,
            CancellationToken cancellationToken = default)
            where TOut : class
        {
            var batches = await _mongoDbCollection.AggregateAsync(pipelineDefinition, null, cancellationToken);
            var items = await batches.ToListAsync(cancellationToken);

            return items;
        }

        public async Task<T> FindOneAsync(
            FilterDefinition<T> filterDefinition,
            CancellationToken cancellationToken = default)
        {
            var item = await _mongoDbCollection
                .Find(filterDefinition)
                .SingleAsync(cancellationToken);
            return item;
        }

        public async Task<TOut> FindOneAsync<TOut>(
            PipelineDefinition<T, TOut> pipelineDefinition,
            CancellationToken cancellationToken = default)
            where TOut : class
        {
            var batches = await _mongoDbCollection.AggregateAsync(pipelineDefinition, null, cancellationToken);
            var item = await batches.SingleAsync(cancellationToken);

            return item;
        }

        public async Task<T> InsertAsync(
            T model,
            CancellationToken cancellationToken = default)
        {
            if (model.Id == null || model.Id == BsonObjectId.Empty)
            {
                model.Id = ObjectId.GenerateNewId();
            }

            await _mongoDbCollection.InsertOneAsync(model, null, cancellationToken);

            return model;
        }

        public async Task<List<T>> InsertManyAsync(
            List<T> models,
            CancellationToken cancellationToken = default)
        {
            var modelsWithEmptyOrNullId = models.Where(m => m.Id == null || m.Id == BsonObjectId.Empty);
            foreach (var modelWithEmptyOrNullId in modelsWithEmptyOrNullId)
            {
                modelWithEmptyOrNullId.Id = ObjectId.GenerateNewId();
            }

            await _mongoDbCollection.InsertManyAsync(models, cancellationToken: cancellationToken);
            return models;
        }

        public async Task<T> UpdateAsync(
            FilterDefinition<T> filterDefinition,
            UpdateDefinition<T> updateDefinition,
            CancellationToken cancellationToken = default)
        {
            return await _mongoDbCollection.FindOneAndUpdateAsync(filterDefinition, updateDefinition, null, cancellationToken);
        }

        public async Task<long> DeleteAsync(
            FilterDefinition<T> filterDefinition,
            CancellationToken cancellationToken = default)
        {
            var deleteResult = await _mongoDbCollection.DeleteOneAsync(filterDefinition, cancellationToken);
            return deleteResult.DeletedCount;
        }

        public async Task<long> ClearAsync(CancellationToken cancellationToken = default)
        {
            var deleteResult = await _mongoDbCollection.DeleteManyAsync(item => true, cancellationToken);
            return deleteResult.DeletedCount;
        }

        public async Task<long> CountAsync(
            FilterDefinition<T> filterDefinition,
            CancellationToken cancellationToken = default)
        {
            return await _mongoDbCollection.CountDocumentsAsync(filterDefinition, null, cancellationToken);
        }

        private string GetCollectionName(Type type)
        {
            var tableAttributes = type.GetCustomAttributes(typeof(TableAttribute), false)
                .Cast<TableAttribute>()
                .ToList();

            if (tableAttributes.Count != 1)
            {
                _logger.LogWarning($"Can't find {nameof(TableAttribute)} in {type.Name} type. Using default collection name");
                return type.Name;
            }

            var tableAttribute = tableAttributes.First();
            return tableAttribute.Name;
        }
    }
}
