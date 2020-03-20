using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HMA.API.AppStart
{
    internal static class AuthStartup
    {
        public static void Init(IApplicationBuilder app)
        {
        }

        public static void Init(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<GoogleOptions>(configuration.GetSection(nameof(GoogleOptions)));
        }
    }
}
