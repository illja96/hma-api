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

            BllDiStartup.Init(services);
            
            DalDiStartup.Init(services, configuration);
        }
    }
}
