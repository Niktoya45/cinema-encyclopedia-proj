

using CinemaDataService.Domain.Aggregates.CinemaAggregate;
using CinemaDataService.Infrastructure.Models.SharedDTO;

namespace CinemaDataService.Infrastructure.Repositories.Abstractions
{
    public interface ICinemaRepository:IEntityRepository<Cinema>
    {
        public Task<StudioRecord?> AddProductionStudio(string cinemaId, StudioRecord studio, CancellationToken ct);
        public Task<Starring?> AddStarring(string? cinemaId, Starring starring, CancellationToken ct);
        public Task<List<Cinema>?> FindByYear(int year, Pagination? pg = default, SortBy? sort = default, CancellationToken ct = default);
        public Task<List<Cinema>?> FindByGenres(Genre genre, Pagination? pg = default, SortBy? sort = default, CancellationToken ct = default);
        public Task<List<Cinema>?> FindByLanguage(Language language, Pagination? pg = default, SortBy? sort = default, CancellationToken ct = default);
        public Task<List<Cinema>?> FindByStudioId(string studioId, Pagination? pg = default, SortBy? sort = default, CancellationToken ct = default);
        public Task<StudioRecord?> FindProductionStudioById(string? cinemaId, string studioId, CancellationToken ct);
        public Task<Starring?> FindStarringById(string? cinemaId, string starringId, CancellationToken ct);
        public Task<StudioRecord?> UpdateProductionStudio(string? cinemaId, StudioRecord studio, CancellationToken ct);
        public Task<Starring?> UpdateStarring(string? cinemaId, Starring starring, CancellationToken ct);
        public Task<Cinema?> UpdateRating(string id, double rating, double oldRating, CancellationToken ct);
        public Task<StudioRecord?> DeleteProductionStudio(string? cinemaId, StudioRecord studio, CancellationToken ct);
        public Task<Starring?> DeleteStarring(string? cinemaId, Starring starring, CancellationToken ct);
    }
}
