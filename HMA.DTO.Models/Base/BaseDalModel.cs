using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HMA.DTO.Models.Base
{
    public abstract class BaseDalModel
    {
        [BsonId]
        public BsonObjectId Id { get; set; }
    }
}
