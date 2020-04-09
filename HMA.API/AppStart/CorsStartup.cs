using HMA.API.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HMA.API.AppStart
{
    public static class CorsStartup
    {
        public static void Init(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CorsOptions>(configuration.GetSection(nameof(CorsOptions)));
        }

        public static void Init(IApplicationBuilder app, IConfiguration configuration)
        {
            app.UseCors(corsPolicy =>
            {
                var corsOptions = configuration.GetSection(nameof(CorsOptions)).Get<CorsOptions>();

                corsPolicy.WithOrigins(corsOptions.AllowedOrigins.ToArray());
                corsPolicy.WithMethods(corsOptions.AllowedMethods.ToArray());
                corsPolicy.WithHeaders(corsOptions.AllowedHeaders.ToArray());
            });
        }
    }
}
