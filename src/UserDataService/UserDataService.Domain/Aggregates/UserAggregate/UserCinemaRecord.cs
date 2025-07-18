
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace UserDataService.Domain.Aggregates.UserAggregate
{
    public record UserCinemaRecord
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonRepresentation(BsonType.String)]
        public string? CinemaId { get; set; }

        [BsonRepresentation(BsonType.String)]
        public string UserId { get; set; }
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    }
}
