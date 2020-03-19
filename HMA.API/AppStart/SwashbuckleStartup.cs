using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace HMA.API.AppStart
{
    internal static class SwashbuckleStartup
    {
        public static void Init(IApplicationBuilder app)
        {
            app.UseSwagger();

            app.UseSwaggerUI(swaggerUi =>
            {
                swaggerUi.RoutePrefix = string.Empty;

                var assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                swaggerUi.SwaggerEndpoint(
                    $"/swagger/{assemblyVersion}/swagger.json",
                    "HMA API");
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
            });
        }
    }
}
