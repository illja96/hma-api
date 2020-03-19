using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HMA.API.AppStart
{
    internal static class OAuthStartup
    {
        public static void Init(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<GoogleOptions>(configuration.GetSection(nameof(GoogleOptions)));
            
            services
                .AddAuthentication()
                .AddGoogle(google =>
                {
                    var googleOptions = configuration.GetSection(nameof(GoogleOptions)).Get<GoogleOptions>();

                    google.ClientId = googleOptions.ClientId;
                    google.ClientSecret = googleOptions.ClientSecret;
                    google.AuthorizationEndpoint = googleOptions.AuthorizationEndpoint;
                    google.TokenEndpoint = googleOptions.TokenEndpoint;
                    google.UserInformationEndpoint = googleOptions.UserInformationEndpoint;
                    
                    google.CallbackPath = googleOptions.CallbackPath;
                });
        }
    }
}
