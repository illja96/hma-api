using HMA.BLL.Tier2.Services;
using HMA.BLL.Tier2.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace HMA.DI.Projects
{
    internal static class BllTier2DiStartup
    {
        public static void Init(IServiceCollection services)
        {
            services.AddSingleton<IHouseT2Service, HouseT2Service>();
            services.AddSingleton<IUserT2Service, UserT2Service>();
            services.AddScoped<ITransactionT2Service, TransactionT2Service>();
        }
    }
}
