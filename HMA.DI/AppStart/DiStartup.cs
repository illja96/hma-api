using HMA.DI.Projects;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HMA.DI.AppStart
{
    public static class DiStartup
    {
        public static void Init(IServiceCollection services, IConfiguration configuration)
        {
            AutoMapperDiStartup.Init(services);

            BllTier3DiStartup.Init(services);
            BllTier2DiStartup.Init(services);
            BllTier1DiStartup.Init(services);
            
            DalDiStartup.Init(services, configuration);
        }
    }
}
