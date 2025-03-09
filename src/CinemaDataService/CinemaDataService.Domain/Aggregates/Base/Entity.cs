
using System.Globalization;
using MongoDB.Bson.Serialization.Attributes;

namespace CinemaDataService.Domain.Aggregates.Base
{
    public class Entity
    {
        [BsonId]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
