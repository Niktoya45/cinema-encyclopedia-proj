
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace UserDataService.Domain.Aggregates.UserAggregate
{
    public record RatingRecord:UserCinemaRecord
    {
        public double Rating { get; set; } = 0.0;
    }
}
