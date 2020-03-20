using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HMA.API.AppStart.Auth
{
    internal static class AuthStartup
    {
        public static void Init(IApplicationBuilder app)
        {
            app.UseAuthentication();
        }

        public static void Init(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<GoogleOptions>(configuration.GetSection(nameof(GoogleOptions)));

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(jwtBearer =>
                {
                    jwtBearer.SaveToken = true;
                    jwtBearer.RequireHttpsMetadata = false;

                    jwtBearer.TokenValidationParameters.ValidateLifetime = true;

                    jwtBearer.TokenValidationParameters.ValidateIssuer = true;
                    jwtBearer.TokenValidationParameters.ValidIssuer = "https://accounts.google.com";

                    var googleOptions = configuration.GetSection(nameof(GoogleOptions)).Get<GoogleOptions>();
                    jwtBearer.TokenValidationParameters.ValidateAudience = true;
                    jwtBearer.TokenValidationParameters.ValidAudience = googleOptions.ClientId;

                    var googlePublicCertificateProvider = new GooglePublicCertificateProvider();
                    var signingKeys = googlePublicCertificateProvider.GetAsync().GetAwaiter().GetResult();
                    jwtBearer.TokenValidationParameters.ValidateIssuerSigningKey = true;
                    jwtBearer.TokenValidationParameters.IssuerSigningKeys = signingKeys;

                    jwtBearer.TokenValidationParameters.NameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress";
                });
        }
    }
}
