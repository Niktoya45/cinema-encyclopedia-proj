using UserDataService.Domain.Aggregates.UserAggregate;
using UserDataService.Infrastructure.Models.SharedDTO;

namespace UserDataService.Infrastructure.Repositories.Abstractions
{
    public interface IUserRepository
    {
        public User Add(User user);
        public Task<LabeledRecord?> AddToLabeledList(LabeledRecord cinema, CancellationToken ct);
        public Task<RatingRecord?> AddToRatingList(RatingRecord rating, CancellationToken ct);
        public Task<User?> Update(User user, CancellationToken ct);
        public Task<User?> FindById(string id, CancellationToken ct);
        public Task<RatingRecord?> FindRatingByUserIdCinemaId(string userId, string cinemaId, CancellationToken ct);
        public Task<List<LabeledRecord>?> FindCinemasByUserId(string userId, Pagination? pg, CancellationToken ct);
        public Task<List<LabeledRecord>?> FindCinemasByUserIdLabel(string userId, Label? label, string? cinemaId, Pagination? pg, CancellationToken ct);
        public Task<List<RatingRecord>?> FindRatingsByUserId(string userId, Pagination? pg, CancellationToken ct);
        public Task<User?> Delete(string id, CancellationToken ct);
        public Task<IEnumerable<LabeledRecord>?> DeleteFromCinemaList(LabeledRecord cinema, CancellationToken ct);
        public Task<IEnumerable<RatingRecord>?> DeleteFromRatingList(RatingRecord rating, CancellationToken ct);
    }
}
