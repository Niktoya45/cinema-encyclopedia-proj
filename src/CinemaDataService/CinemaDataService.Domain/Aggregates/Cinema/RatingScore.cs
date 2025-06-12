

using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace CinemaDataService.Domain.Aggregates.CinemaAggregate
{
    public record RatingScore
    {
        [BsonRepresentation(BsonType.Double)]
        public double Score { get; set; } = 0.0;

        [BsonRepresentation(BsonType.Int32)]
        public uint N { get; set; } = 0;
    }
}
