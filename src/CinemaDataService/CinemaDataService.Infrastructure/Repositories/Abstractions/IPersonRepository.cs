
using CinemaDataService.Domain.Aggregates.PersonAggregate;
using CinemaDataService.Domain.Aggregates.Shared;
using CinemaDataService.Infrastructure.Pagination;
using CinemaDataService.Infrastructure.Sort;

namespace CinemaDataService.Infrastructure.Repositories.Abstractions
{
    public interface IPersonRepository:IEntityRepository<Person>
    {
        public Task<CinemaRecord?> AddToFilmography(string? personId, CinemaRecord cinema, CancellationToken ct = default);
        public Task<List<Person>?> FindByJobs(Job jobs, Pagination.Pagination? pg = default, SortBy? st = default,  CancellationToken ct = default);
        public Task<List<Person>?> FindByCountry(Country country, Pagination.Pagination? pg = default, SortBy? st = default, CancellationToken ct = default);
        public Task<CinemaRecord?> UpdateFilmography(CinemaRecord cinema, CancellationToken ct = default);
        public Task<CinemaRecord?> DeleteFromFilmography(string? personId, CinemaRecord cinema, CancellationToken ct = default);
    }
}
