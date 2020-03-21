using HMA.BLL.Wrappers.Wrappers;
using HMA.BLL.Wrappers.Wrappers.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace HMA.DI.Projects
{
    internal static class BllWrappersDiStartup
    {
        public static void Init(IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.AddScoped<IWrappedUserService, WrappedUserService>();
        }
    }
}
