
using Amazon.Auth.AccessControlPolicy;
using MongoDB.Driver;
using System.Linq.Expressions;
using UserDataService.Domain.Aggregates.UserAggregate;
using UserDataService.Infrastructure.Context;
using UserDataService.Infrastructure.Models.SharedDTO;
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
        public async Task<LabeledRecord?> AddToLabeledList(LabeledRecord cinema, CancellationToken ct = default)
        {
            User? user = await FindById(cinema.UserId, ct);

            if (user == null)
            {
                return null;
            }

            var findIfExists = Builders<LabeledRecord>.Filter.Where(r => (r.UserId == cinema.UserId) && (r.Id == cinema.Id));
            var updateLabel = Builders<LabeledRecord>.Update.Set(r => r.Label, cinema.Label);

            var res = await _db.LabeledRecords.UpdateOneAsync(findIfExists, updateLabel, new UpdateOptions { IsUpsert = true }, ct);

            if (!res.IsAcknowledged)
            {
                return null;
            }

            return cinema;
        }

        public async Task<RatingRecord?> AddToRatingList(RatingRecord rating, CancellationToken ct = default) 
        {
            User? user = await FindById(rating.UserId, ct);

            if (user == null)
            {
                return null;
            }

            var findIfExists = Builders<RatingRecord>.Filter.Where(r => (r.UserId == rating.UserId) && (r.Id == rating.Id));
            var updateLabel = Builders<RatingRecord>.Update.Set(r => r.Rating, rating.Rating);

            var res = await _db.RatingRecords.UpdateOneAsync(findIfExists, updateLabel, new UpdateOptions { IsUpsert = true }, ct);

            if (!res.IsAcknowledged)
            {
                return null;
            }

            return rating;
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

        public async Task<RatingRecord?> FindRatingByUserIdCinemaId(string userId, string cinemaId, CancellationToken ct = default)
        {
            var condition = Builders<RatingRecord>.Filter.Where(r => r.UserId == userId && r.CinemaId == cinemaId);
            var sort = Builders<RatingRecord>.Sort.Descending(r => r.AddedAt);

            return await _db.RatingRecords.Find(condition).Sort(sort).FirstOrDefaultAsync(ct);
        }

        public async Task<List<LabeledRecord>?> FindCinemasList(Pagination? pg = default, Expression<Func<LabeledRecord, bool>>? query = null, CancellationToken ct = default) {
            
            var condition = Builders<LabeledRecord>.Filter.Where(query??(r => true));
            var sort = Builders<LabeledRecord>.Sort.Descending(r => r.AddedAt);

            int skip = pg == null ? 0 : pg.Skip;
            int take = pg == null ? 0 : pg.Take;

            return await _db.LabeledRecords.Find(condition).Sort(sort).Skip(skip).Limit(take).ToListAsync(ct);
        }

        public async Task<List<RatingRecord>?> FindRatingsList(Pagination? pg = default, Expression<Func<RatingRecord, bool>>? query = null, CancellationToken ct = default)
        {

            var condition = Builders<RatingRecord>.Filter.Where(query ?? (r => true));
            var sort = Builders<RatingRecord>.Sort.Descending(r => r.AddedAt);

            int skip = pg == null ? 0 : pg.Skip;
            int take = pg == null ? 0 : pg.Take;

            return await _db.RatingRecords.Find(condition).Sort(sort).Skip(skip).Limit(take).ToListAsync(ct);
        }

        public async Task<List<LabeledRecord>?> FindCinemasByUserId(string userId, Pagination? pg = default, CancellationToken ct = default)
        {
            return await FindCinemasList(pg, r => (r.UserId == userId), ct);
        }

        public async Task<List<LabeledRecord>?> FindCinemasByUserIdLabel(string userId, Label? label, Pagination? pg = default, CancellationToken ct = default)
        {
            return await FindCinemasList(
                                        pg,
                                    r => (r.UserId == userId) && (label == null ? true : (r.Label & label) != 0),
                                        ct
                                        );
        }

        public async Task<List<RatingRecord>?> FindRatingsByUserId(string userId, Pagination? pg = default, CancellationToken ct = default)
        {
            return await FindRatingsList(pg, r => (r.UserId == userId), ct);
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
            User? found = await FindById(id, ct);

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

        public async Task<LabeledRecord?> DeleteFromCinemaList(LabeledRecord cinema, CancellationToken ct = default)
        {
            var findIfExists = Builders<LabeledRecord>.Filter.Where(r => (r.UserId == cinema.UserId) && (r.CinemaId == cinema.CinemaId));
            var sortByTimeAdded = Builders<LabeledRecord>.Sort.Descending(r => r.AddedAt);

            LabeledRecord found = await _db.LabeledRecords.Find(findIfExists).Sort(sortByTimeAdded).FirstOrDefaultAsync(ct);

            var res = await _db.LabeledRecords.DeleteOneAsync(findIfExists, ct);

            if (!res.IsAcknowledged)
            {
                return null;
            }

            return found;
        }

        public async Task<RatingRecord?> DeleteFromRatingList(RatingRecord rating, CancellationToken ct = default)
        {
            var findIfExists = Builders<RatingRecord>.Filter.Where(r => (r.UserId == rating.UserId) && (r.CinemaId == rating.CinemaId));
            var sortByTimeAdded = Builders<RatingRecord>.Sort.Descending(r => r.AddedAt);

            RatingRecord found = await _db.RatingRecords.Find(findIfExists).Sort(sortByTimeAdded).FirstOrDefaultAsync(ct);

            var res = await _db.RatingRecords.DeleteOneAsync(findIfExists, ct);

            if (!res.IsAcknowledged)
            {
                return null;
            }

            return found;
        }
    }
}
