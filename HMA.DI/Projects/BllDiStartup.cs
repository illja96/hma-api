using HMA.BLL.Tier1.Services;
using HMA.BLL.Tier1.Services.Interfaces;
using HMA.BLL.Tier2.Services;
using HMA.BLL.Tier2.Services.Interfaces;
using HMA.BLL.Tier3.Services;
using HMA.BLL.Tier3.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace HMA.DI.Projects
{
    internal static class BllDiStartup
    {
        public static void Init(IServiceCollection services)
        {
            services.AddSingleton<IHouseT1Service, HouseT1Service>();
            services.AddSingleton<IUserT1Service, UserT1Service>();

            services.AddSingleton<IHouseT2Service, HouseT2Service>();
            services.AddSingleton<IUserT2Service, UserT2Service>();

            services.AddHttpContextAccessor();

            services.AddSingleton<IHouseT3Service, HouseT3Service>();
            services.AddSingleton<IUserT3Service, UserT3Service>();
        }
    }
}
