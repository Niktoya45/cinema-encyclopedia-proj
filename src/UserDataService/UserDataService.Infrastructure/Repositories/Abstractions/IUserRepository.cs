using UserDataService.Domain.Aggregates.UserAggregate;

namespace UserDataService.Infrastructure.Repositories.Abstractions
{
    public interface IUserRepository
    {
        public User Add(User user);
        public Task<CinemaRecord?> AddToCinemaList(CinemaRecord cinema, CancellationToken ct);
        public Task<User?> Update(User user, CancellationToken ct);
        public Task<User?> FindById(string id, CancellationToken ct);
        public Task<List<CinemaRecord>?> FindCinemaByUserId(string userId, Pagination.Pagination? pg, CancellationToken ct);
        public Task<List<CinemaRecord>?> FindCinemaByUserIdLabel(string userId, Label label, Pagination.Pagination? pg, CancellationToken ct);
        public Task<User?> Delete(string id, CancellationToken ct);
        public Task<CinemaRecord?> DeleteFromCinemaList(CinemaRecord cinema, CancellationToken ct);
    }
}
