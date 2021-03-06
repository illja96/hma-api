using System.Text.Json.Serialization;
using HMA.API.AppStart;
using HMA.API.AppStart.Auth;
using HMA.API.AppStart.Swashbuckle;
using HMA.DI.AppStart;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace HMA.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            SwashbuckleStartup.Init(services, Configuration);

            DiStartup.Init(services, Configuration);

            AuthStartup.Init(services, Configuration);

            CorsStartup.Init(services, Configuration);

            services
                .AddControllers()
                .AddJsonOptions(jsonOptions =>
                {
                    var jsonStringEnumConverter = new JsonStringEnumConverter();
                    jsonOptions.JsonSerializerOptions.Converters.Add(jsonStringEnumConverter);
                });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            SwashbuckleStartup.Init(app, Configuration);

            AuthStartup.Init(app);

            CorsStartup.Init(app, Configuration);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHsts();

            app.UseSerilogRequestLogging();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
