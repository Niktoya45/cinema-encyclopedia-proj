using CinemaDataService.Domain.Aggregates.Base;
using CinemaDataService.Domain.Aggregates.Shared;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace CinemaDataService.Domain.Aggregates.StudioAggregate
{
    public class Studio : Entity
    {
        public DateOnly FoundDate {  get; set; }
        [BsonRepresentation(BsonType.Int32)]
        public Country Country { get; set; }

        [BsonElement("Filmography")]
        public IList<CinemaRecord>? Filmography { get; set; }
        public string? PresidentName { get; set; }
    }
}
