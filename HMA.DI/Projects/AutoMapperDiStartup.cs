using System.Collections.Generic;
using System.Reflection;
using AutoMapper;
using HMA.AutoMapper.Profiles;
using Microsoft.Extensions.DependencyInjection;

namespace HMA.DI.Projects
{
    internal static class AutoMapperDiStartup
    {
        public static void Init(IServiceCollection services)
        {
            var profiles = new List<Profile>()
            {
                new UserProfile()
            };

            var assemblies = new List<Assembly>()
            {
                Assembly.GetExecutingAssembly()
            };

            services.AddAutoMapper(
                (provider, expression) => expression.AddProfiles(profiles),
                assemblies);
        }
    }
}
