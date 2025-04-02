
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace CinemaDataService.Domain.Aggregates.Base
{
    public record Value
    {
        [BsonId]
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Picture { get; set; } = default(string);
    }
}
