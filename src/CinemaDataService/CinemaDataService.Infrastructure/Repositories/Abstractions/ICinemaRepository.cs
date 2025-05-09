﻿

using CinemaDataService.Domain.Aggregates.CinemaAggregate;
using CinemaDataService.Infrastructure.Repositories.Utils;

namespace CinemaDataService.Infrastructure.Repositories.Abstractions
{
    public interface ICinemaRepository:IEntityRepository<Cinema>
    {
        public Task<StudioRecord?> AddProductionStudio(string cinemaId, StudioRecord studio, CancellationToken ct = default);
        public Task<Starring?> AddStarring(string? cinemaId, Starring starring, CancellationToken ct = default);
        public Task<List<Cinema>?> FindByYear(int year, Pagination? pg = default, SortBy? sort = default, CancellationToken ct = default);
        public Task<List<Cinema>?> FindByGenres(Genre genre, Pagination? pg = default, SortBy? sort = default, CancellationToken ct = default);
        public Task<List<Cinema>?> FindByLanguage(Language language, Pagination? pg = default, SortBy? sort = default, CancellationToken ct = default);
        public Task<List<Cinema>?> FindByStudioId(string studioId, Pagination? pg = default, SortBy? sort = default, CancellationToken ct = default);
        public Task<StudioRecord?> UpdateProductionStudio(StudioRecord studio, CancellationToken ct = default);
        public Task<Starring?> UpdateStarring(Starring starring, CancellationToken ct = default);
        public Task<StudioRecord?> DeleteProductionStudio(string? cinemaId, StudioRecord studio, CancellationToken ct = default);
        public Task<Starring?> DeleteStarring(string? cinemaId, Starring starring, CancellationToken ct = default);
    }
}
