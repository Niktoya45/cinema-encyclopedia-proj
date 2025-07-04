
using CinemaDataService.Domain.Aggregates.Shared;
using CinemaDataService.Domain.Aggregates.StudioAggregate;
using CinemaDataService.Infrastructure.Models.SharedDTO;

namespace CinemaDataService.Infrastructure.Repositories.Abstractions
{
    public interface IStudioRepository:IEntityRepository<Studio>
    {
        public Task<CinemaRecord?> AddToFilmography(string studioId, CinemaRecord cinema, CancellationToken ct = default);
        public Task<List<Studio>?> FindByYear(int year, Pagination? pg = default, SortBy? st = default, CancellationToken ct=default);
        public Task<List<Studio>?> FindByYearSpans(int[] yearsLower, int yearSpan, Pagination? pg = default, SortBy? st = default, CancellationToken ct=default);
        public Task<List<Studio>?> FindByCountry(Country country, Pagination? pg = default, SortBy? st = default, CancellationToken ct=default);
        public Task<CinemaRecord?> FindFilmographyById(string? studioId, string filmographyId, CancellationToken ct);
        public Task<CinemaRecord?> UpdateFilmography(string? studioId, CinemaRecord cinema, CancellationToken ct = default);
        public Task<CinemaRecord?> DeleteFromFilmography(string? studioId, CinemaRecord cinema, CancellationToken ct = default);
    }
}
