using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

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
                    var googleOptions = configuration.GetSection(nameof(GoogleOptions)).Get<GoogleOptions>();
                    var googlePublicCertificateProvider = new GooglePublicCertificateProvider();
                    var signingKeys = googlePublicCertificateProvider.GetAsync().GetAwaiter().GetResult();

                    jwtBearer.RequireHttpsMetadata = false;

                    jwtBearer.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateLifetime = true,

                        ValidateIssuer = true,
                        ValidIssuer = "https://accounts.google.com",

                        ValidateAudience = true,
                        ValidAudience = googleOptions.ClientId,

                        ValidateIssuerSigningKey = true,
                        IssuerSigningKeys = signingKeys
                    };
                });
        }
    }
}
