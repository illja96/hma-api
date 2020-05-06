using HMA.BLL.Tier1.Services;
using HMA.BLL.Tier1.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace HMA.DI.Projects
{
    internal static class BllTier1DiStartup
    {
        public static void Init(IServiceCollection services)
        {
            services.AddSingleton<IHouseT1Service, HouseT1Service>();
            services.AddSingleton<IUserT1Service, UserT1Service>();
            services.AddScoped<ITransactionT1Service, TransactionT1Service>();
        }
    }
}
