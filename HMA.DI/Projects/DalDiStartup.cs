using HMA.DAL.Factories;
using HMA.DAL.Options;
using HMA.DAL.Repositories;
using HMA.DAL.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace HMA.DI.Projects
{
    internal static class DalDiStartup
    {
        public static void Init(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoDbOptions>(configuration.GetSection(nameof(MongoDbOptions)));

            services.AddSingleton(serviceProvider =>
            {
                var mongoDbOptions = serviceProvider.GetRequiredService<IOptions<MongoDbOptions>>();
                var logger = serviceProvider.GetRequiredService<ILogger<MongoClient>>();

                var mongoClient = MongoClientFactory.Create(mongoDbOptions.Value, logger);
                return mongoClient;
            });

            services.AddSingleton(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        }
    }
}
