using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace UserDataService.Domain.Aggregates.UserAggregate
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Username { get; set; }
        public DateOnly Birthdate { get; set; }
        public string Picture { get; set; }
    }
}
