using HMA.DTO.Models.House;
using MongoDB.Driver;

namespace HMA.DAL.Factories
{
    public static class BuilderFactory
    {
        public static FilterDefinition<HouseInfo> CreateHouseOwnerOrMembershipFilter(decimal userId)
        {
            var ownerFilter = Builders<HouseInfo>.Filter.Eq(hi => hi.OwnerId, userId);
            var membershipFilter = Builders<HouseInfo>.Filter.All(hi => hi.MemberIds, new[] { userId });

            var userFilter = Builders<HouseInfo>.Filter.Or(ownerFilter, membershipFilter);
            return userFilter;
        }
    }
}
