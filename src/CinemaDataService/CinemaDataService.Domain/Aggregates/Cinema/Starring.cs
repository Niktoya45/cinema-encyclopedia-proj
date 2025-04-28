using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using CinemaDataService.Domain.Aggregates.Base;
using CinemaDataService.Domain.Aggregates.Shared;

namespace CinemaDataService.Domain.Aggregates.CinemaAggregate
{
    public record Starring:Value
    {
        [BsonRepresentation(BsonType.Int32)]
        public Job Jobs { get; set; } 
        public ActorRole? ActorRole { get; set; }
    }
}
