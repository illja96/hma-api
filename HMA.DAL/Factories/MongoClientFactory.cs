using HMA.DAL.Options;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;

namespace HMA.DAL.Factories
{
    public static class MongoClientFactory
    {
        public static IMongoClient Create(MongoDbOptions mongoDbOptions, ILogger<MongoClient> logger)
        {
            var mongoConnectionUrl = new MongoUrl(mongoDbOptions.ConnectionString);
            var mongoClientSettings = MongoClientSettings.FromUrl(mongoConnectionUrl);
            mongoClientSettings.ClusterConfigurator = cb =>
            {
                cb.Subscribe<CommandStartedEvent>(e =>
                {
                    logger.LogDebug($"{e.CommandName} - {e.Command.ToJson()}");
                });
            };

            var mongoClient = new MongoClient(mongoClientSettings);
            return mongoClient as IMongoClient;
        }
    }
}
