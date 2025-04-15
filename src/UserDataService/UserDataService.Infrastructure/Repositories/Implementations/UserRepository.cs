
using MongoDB.Driver;
using System.Linq.Expressions;
using UserDataService.Domain.Aggregates.UserAggregate;
using UserDataService.Infrastructure.Context;
using UserDataService.Infrastructure.Repositories.Abstractions;

namespace UserDataService.Infrastructure.Repositories.Implementations
{
    public class UserRepository:IUserRepository
    {
        protected UserDataDb _db { get; set; }

        public UserRepository(UserDataDb db)
        {
            _db = db;
        }

        public User Add(User user)
        {
            _db.Users.InsertOne(user);

            return user;
        }
        public async Task<CinemaRecord?> AddToCinemaList(CinemaRecord cinema, CancellationToken ct = default)
        {
            User? user = await FindById(cinema.UserId);

            if (user == null)
            {
                return null;
            }

            var findIfExists = Builders<CinemaRecord>.Filter.Where(r => (r.UserId == cinema.UserId) && (r.Id == cinema.Id));
            var updateLabel = Builders<CinemaRecord>.Update.Set(r => r.Label, cinema.Label);

            var res = await _db.CinemaRecords.UpdateOneAsync(findIfExists, updateLabel, new UpdateOptions { IsUpsert = true }, ct);

            if (!res.IsAcknowledged)
            {
                return null;
            }

            return cinema;
        }

        public async Task<User?> FindOne(Expression<Func<User, bool>>? query = null, CancellationToken ct = default)
        {
            var condition = Builders<User>.Filter.Where(query ?? (e => true));
            var notDeleted = Builders<User>.Filter.Where(u => !u.IsDeleted);

            var sortByTimeCreated = Builders<User>.Sort.Descending(u => u.CreatedAt);

            return await _db.Users.Find(condition & notDeleted).Sort(sortByTimeCreated).FirstOrDefaultAsync(ct);
        }
        public async Task<User?> FindById(string id, CancellationToken ct = default)
        {
            return await FindOne(u => u.Id == id, ct);
        }

        public async Task<List<CinemaRecord>?> FindCinemaList(Pagination.Pagination? pg = default, Expression<Func<CinemaRecord, bool>>? query = null, CancellationToken ct = default) {
            
            var condition = Builders<CinemaRecord>.Filter.Where(query??(r => true));
            var sort = Builders<CinemaRecord>.Sort.Descending(r => r.AddedAt);

            return await _db.CinemaRecords.Find(condition).Sort(sort).Skip(pg.Skip).Limit(pg.Take).ToListAsync(ct);
        }

        public async Task<List<CinemaRecord>?> FindCinemaByUserId(string userId, Pagination.Pagination? pg = default, CancellationToken ct = default)
        {
            return await FindCinemaList(pg, r => (r.UserId == userId), ct);
        }

        public async Task<List<CinemaRecord>?> FindCinemaByUserIdLabel(string userId, Label label, Pagination.Pagination? pg = default, CancellationToken ct = default)
        {
            return await FindCinemaList(
                                        pg,
                                    r => (r.UserId == userId) && (r.Label & label) != 0,
                                        ct
                                        );
        }

        public async Task<User?> Update(User user, CancellationToken ct = default) {
            UpdateDefinitionBuilder<User> builder = Builders<User>.Update;

            UpdateDefinition<User> Set<TField>(Expression<Func<User, TField>> field, TField value) => builder.Set(field, value);

            var update = Builders<User>.Update.Combine(
                    Set(u => u.Username, user.Username),
                    Set(u => u.Birthdate, user.Birthdate),
                    Set(u => u.Picture, user.Picture)
                );

            var find = Builders<User>.Filter.Where(u => u.Id == user.Id);

            var res = await _db.Users.UpdateOneAsync(find, update, new UpdateOptions { IsUpsert = false }, ct);

            if (!res.IsAcknowledged)
            {
                return null;
            }

            return user;
        }

        public async Task<User?> Delete(string id, CancellationToken ct = default)
        {
            User? found = await FindById(id);

            if (found == null)
            {
                return null;
            }
            _db.Users.UpdateOne(
                Builders<User>.Filter.Where(u => u.Id == id),
                Builders<User>.Update.Set(u => u.IsDeleted, true)
                );
            found.IsDeleted = true;

            return found;
        }

        public async Task<CinemaRecord?> DeleteFromCinemaList(CinemaRecord cinema, CancellationToken ct)
        {
            var findIfExists = Builders<CinemaRecord>.Filter.Where(r => (r.UserId == cinema.UserId) && (r.Id == cinema.Id));

            var res = await _db.CinemaRecords.DeleteOneAsync(findIfExists, ct);

            if (!res.IsAcknowledged) {
                return null;
            }

            return cinema;
        }
    }
}
