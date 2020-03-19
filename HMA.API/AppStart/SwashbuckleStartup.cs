using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace HMA.API.AppStart
{
    internal static class SwashbuckleStartup
    {
        public static void Init(IApplicationBuilder app, IConfiguration configuration)
        {
            app.UseSwagger();

            app.UseSwaggerUI(swaggerUi =>
            {
                swaggerUi.RoutePrefix = string.Empty;

                var assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                swaggerUi.SwaggerEndpoint(
                    $"/swagger/{assemblyVersion}/swagger.json",
                    "HMA API");

                 var googleOptions = configuration.GetSection(nameof(GoogleOptions)).Get<GoogleOptions>();
                swaggerUi.OAuthClientId(googleOptions.ClientId);
                swaggerUi.OAuthAppName("House Money Accountant");
            });
        }

        public static void Init(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<OpenApiInfo>(configuration.GetSection(nameof(OpenApiInfo)));

            services.AddSwaggerGen(swaggerGen =>
            {
                var assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();

                var openApiInfo = configuration.GetSection(nameof(OpenApiInfo)).Get<OpenApiInfo>();
                openApiInfo.Version = assemblyVersion;

                swaggerGen.SwaggerDoc(assemblyVersion, openApiInfo);

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                swaggerGen.IncludeXmlComments(xmlPath);

                swaggerGen.AddSecurityDefinition(
                    "google-oauth2",
                    new OpenApiSecurityScheme()
                    {
                        Type = SecuritySchemeType.OAuth2,
                        Name = "google-oauth2",
                        Description = "Google OAuth 2",
                        Flows = new OpenApiOAuthFlows()
                        {
                            Implicit = new OpenApiOAuthFlow()
                            {
                                AuthorizationUrl = new Uri("https://accounts.google.com/o/oauth2/v2/auth"),
                                TokenUrl = new Uri("https://oauth2.googleapis.com/token"),
                                RefreshUrl = new Uri("https://oauth2.googleapis.com/token"),
                                Scopes = new Dictionary<string, string>()
                                {
                                    { "openid", "OpenID info" },
                                    { "email", "Email" },
                                    { "profile", "Profile" }
                                }
                            }
                        }
                    });
            });
        }
    }
}
