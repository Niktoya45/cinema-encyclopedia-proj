
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace UserDataService.Domain.Aggregates.UserAggregate
{
    public record UserCinemaRecord
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonRepresentation(BsonType.String)]
        public string? CinemaId { get; set; }
        public string UserId { get; set; }
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    }
}
