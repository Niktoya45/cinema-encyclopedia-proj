using CinemaDataService.Domain.Aggregates.Base;
using CinemaDataService.Domain.Aggregates.Cinema;
using CinemaDataService.Domain.Aggregates.Shared;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace CinemaDataService.Domain.Aggregates.Studio
{
    public class Studio : Entity
    {
        public int Year { get; set; }

        [BsonRepresentation(BsonType.Int32)]
        public Country Country { get; set; }
        public IList<CinemaRecord> Filmography { get; set; }
        public string PresidentName { get; set; }
        public string History { get; set; }
    }
}
