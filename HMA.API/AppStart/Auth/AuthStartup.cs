using HMA.Infrastructure.Auth;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace HMA.API.AppStart.Auth
{
    internal static class AuthStartup
    {
        public static void Init(IApplicationBuilder app)
        {
            app.UseAuthentication();

            app.UseAuthorization();
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
                    jwtBearer.TokenValidationParameters.ValidIssuers = new List<string>()
                    {
                        "https://accounts.google.com",
                        "accounts.google.com"
                    };

                    var googleOptions = configuration.GetSection(nameof(GoogleOptions)).Get<GoogleOptions>();
                    jwtBearer.TokenValidationParameters.ValidateAudience = true;
                    jwtBearer.TokenValidationParameters.ValidAudience = googleOptions.ClientId;

                    var googlePublicCertificateProvider = new GooglePublicCertificateProvider();
                    var signingKeys = googlePublicCertificateProvider.GetAsync().GetAwaiter().GetResult();
                    jwtBearer.TokenValidationParameters.ValidateIssuerSigningKey = true;
                    jwtBearer.TokenValidationParameters.IssuerSigningKeys = signingKeys;

                    jwtBearer.TokenValidationParameters.NameClaimType = ClaimsConstants.NameIdentifier;
                });

            services.AddTransient<IAuthorizationHandler, MongoAuthorizationHandler>();

            services.AddAuthorization(authorization =>
            {
                authorization.AddPolicy(
                    PolicyConstants.UserRegistered,
                    builder =>
                    {
                        builder.RequireClaim(ClaimsConstants.Registered, true.ToString());
                    });
            });
        }
    }
}
