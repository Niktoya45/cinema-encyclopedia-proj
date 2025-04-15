using CinemaDataService.Domain.Aggregates.Base;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Globalization;
using MongoDB.Driver.Search;

namespace CinemaDataService.Domain.Aggregates.CinemaAggregate
{
    public class Cinema:Entity
    {
        public DateOnly ReleaseDate { get; set; }

        [BsonRepresentation(BsonType.Int32)]
        public Genre Genres { get; set; }

        [BsonRepresentation(BsonType.Int32)]
        public Language Language { get; set; }
        public double RatingScore { get; set; } = 0.0;

        [BsonElement("ProductionStudios")]
        public IList<StudioRecord>? ProductionStudios { get; set; }

        [BsonElement("Starrings")]
        public IList<Starring>? Starrings { get; set; }
    }
}
