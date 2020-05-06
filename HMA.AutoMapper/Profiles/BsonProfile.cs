using AutoMapper;
using MongoDB.Bson;

namespace HMA.AutoMapper.Profiles
{
    public class BsonProfile : Profile
    {
        public BsonProfile()
        {
            CreateMap<string, BsonObjectId>()
                .ConvertUsing(src => ObjectId.Parse(src));
        }
    }
}
