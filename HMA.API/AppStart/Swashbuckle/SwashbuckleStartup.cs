using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using HMA.Infrastructure.Auth;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Models;

namespace HMA.API.AppStart.Swashbuckle
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
                swaggerUi.OAuthClientSecret(googleOptions.ClientSecret);
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
                swaggerGen.OperationFilter<AuthorizeOperationFilter>();

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                swaggerGen.IncludeXmlComments(xmlPath);

                swaggerGen.AddSecurityDefinition(
                    "Google OAuth 2.0",
                    new OpenApiSecurityScheme()
                    {
                        Type = SecuritySchemeType.OAuth2,
                        Flows = new OpenApiOAuthFlows()
                        {
                            Implicit = new OpenApiOAuthFlow()
                            {
                                AuthorizationUrl = new Uri("/swagger/fake-oauth", UriKind.Relative),
                                Scopes = ScopeConstants.Scopes
                                    .ToDictionary(
                                        scope => scope,
                                        scope => string.Empty)
                            }
                        },
                        Extensions = new Dictionary<string, IOpenApiExtension>()
                        {
                            { "x-tokenName", new OpenApiString("id_token") }
                        }
                    });

                swaggerGen.AddSecurityDefinition(
                    "Google ID token",
                    new OpenApiSecurityScheme()
                    {
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer",
                        BearerFormat = "JWT"
                    });
            });
        }
    }
}
