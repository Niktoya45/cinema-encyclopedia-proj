
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace CinemaDataService.Domain.Aggregates.Base
{
    public record Value
    {
        [BsonId]
        [BsonElement("Id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Picture { get; set; } = default(string);
    }
}
