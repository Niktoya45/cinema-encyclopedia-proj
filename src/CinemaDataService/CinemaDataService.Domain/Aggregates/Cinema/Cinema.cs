using CinemaDataService.Domain.Aggregates.Base;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Globalization;

namespace CinemaDataService.Domain.Aggregates.Cinema
{
    public class Cinema:Entity
    {
        public int Year { get; set; }

        [BsonRepresentation(BsonType.Int32)]
        public Genre Genres { get; set; }

        [BsonRepresentation(BsonType.Int32)]
        public Language Language { get; set; }
        public IList<StudioRecord> ProductionStudios { get; set; }
        public IList<Starring> Starrings { get; set; }
    }
}
