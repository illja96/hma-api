using HMA.BLL.Tier3.Services;
using HMA.BLL.Tier3.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace HMA.DI.Projects
{
    internal static class BllTier3DiStartup
    {
        public static void Init(IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.AddScoped<IHouseT3Service, HouseT3Service>();
            services.AddScoped<IHouseInviteT3Service, HouseInviteT3Service>();

            services.AddScoped<IUserT3Service, UserT3Service>();

            services.AddScoped<ITransactionT3Service, TransactionT3Service>();
        }
    }
}
