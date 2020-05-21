using AutoMapper;
using HMA.AutoMapper.Profiles;
using System;
using System.Linq;

namespace HMA.AutoMapper.Tests.Factory
{
    public static class AutoMapperFactory
    {
        private static readonly Type[] ProfileTypes = new Type[]
        {
            typeof(BsonProfile),
            typeof(HouseProfile),
            typeof(HouseInviteProfile),
            typeof(UserProfile),
            typeof(TransactionProfile)
        };

        public static IMapper Create()
        {
            var profiles = ProfileTypes
                .Select(pt => Activator.CreateInstance(pt) as Profile)
                .ToList();

            var mapperConfiguration = new MapperConfiguration(mapperCfg => mapperCfg.AddProfiles(profiles));

            var mapper = mapperConfiguration.CreateMapper();

            return mapper;
        }
    }
}
