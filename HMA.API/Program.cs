using System;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace HMA.API
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args)
                .Build()
                .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host
                .CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
                .UseSerilog((hostingContext, loggerConfiguration) =>
                {
                    var assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                    loggerConfiguration.Enrich.WithProperty(nameof(Version), assemblyVersion);

                    loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration);
                });
    }
}
