using CinemaDataService.Domain.Aggregates.Base;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace CinemaDataService.Domain.Aggregates.CinemaAggregate
{
    public class Cinema:Entity
    {
        public DateOnly ReleaseDate { get; set; }

        [BsonRepresentation(BsonType.Int32)]
        public Genre Genres { get; set; }

        [BsonRepresentation(BsonType.Int32)]
        public Language Language { get; set; }

        [BsonElement("Rating")]
        public RatingScore Rating { get; set; } = new RatingScore();

        [BsonElement("ProductionStudios")]
        public IList<StudioRecord>? ProductionStudios { get; set; }

        [BsonElement("Starrings")]
        public IList<Starring>? Starrings { get; set; }
    }
}
