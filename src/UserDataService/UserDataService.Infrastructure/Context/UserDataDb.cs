using MongoDB.Driver;
using UserDataService.Domain.Aggregates.UserAggregate;


namespace UserDataService.Infrastructure.Context
{
    public class UserDataDb
    {
        public IMongoDatabase _mongodb { get; init; }
        public IMongoCollection<User> Users { get; init; }
        public IMongoCollection<CinemaRecord> CinemaRecords { get; init; }

        public UserDataDb(IMongoDatabase mongodb)
        {
            _mongodb = mongodb;

            CinemaRecords = _mongodb.GetCollection<CinemaRecord>("cinema_records");
            Users = _mongodb.GetCollection<User>("users");
        }
    }
}
