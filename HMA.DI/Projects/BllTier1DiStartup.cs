using HMA.BLL.Tier1.Options;
using HMA.BLL.Tier1.Services;
using HMA.BLL.Tier1.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HMA.DI.Projects
{
    internal static class BllTier1DiStartup
    {
        public static void Init(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<HouseInviteOptions>(configuration.GetSection(nameof(HouseInviteOptions)));

            services.AddSingleton<IHouseT1Service, HouseT1Service>();
            services.AddSingleton<IHouseInviteT1Service, HouseInviteT1Service>();

            services.AddSingleton<IUserT1Service, UserT1Service>();

            services.AddScoped<ITransactionT1Service, TransactionT1Service>();
        }
    }
}
