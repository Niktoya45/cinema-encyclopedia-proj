using MongoDB.Driver;
using UserDataService.Domain.Aggregates.UserAggregate;


namespace UserDataService.Infrastructure.Context
{
    public class UserDataDb
    {
        public IMongoDatabase _mongodb { get; init; }
        public IMongoCollection<User> Users { get; init; }
        public IMongoCollection<LabeledRecord> LabeledRecords { get; init; }
        public IMongoCollection<RatingRecord> RatingRecords { get; init; }

        public UserDataDb(IMongoDatabase mongodb)
        {
            _mongodb = mongodb;

            LabeledRecords = _mongodb.GetCollection<LabeledRecord>("labeled_records");
            RatingRecords = _mongodb.GetCollection<RatingRecord>("rating_records");
            Users = _mongodb.GetCollection<User>("users");
        }
    }
}
