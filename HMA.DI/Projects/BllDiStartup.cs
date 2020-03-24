using HMA.BLL.Services;
using HMA.BLL.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace HMA.DI.Projects
{
    internal static class BllDiStartup
    {
        public static void Init(IServiceCollection services)
        {
            services.AddSingleton<IHouseService, HouseService>();
            services.AddSingleton<IUserService, UserService>();
        }
    }
}
