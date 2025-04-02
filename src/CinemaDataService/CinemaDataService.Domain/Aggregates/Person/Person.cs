using CinemaDataService.Domain.Aggregates.Base;
using CinemaDataService.Domain.Aggregates.Shared;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace CinemaDataService.Domain.Aggregates.PersonAggregate
{
    public class Person:Entity
    {
        public DateOnly BirthDate { get; set; } 
        public Country Country { get; set; }

        [BsonRepresentation(BsonType.Int32)]
        public Job Jobs { get; set; }
        public IList<CinemaRecord>? Filmography { get; set; }
    }
}
