
using Shared.CinemaDataService.Models.CinemaDTO;
using Shared.CinemaDataService.Models.RecordDTO;
using Shared.CinemaDataService.Models.SharedDTO;
using Shared.CinemaDataService.Models.Flags;

namespace GatewayAPIService.Infrastructure.Services.CinemaDataService
{
    public interface ICinemaService
    {
        /* Get Requests For Cinema */
        Task<Page<CinemasResponse>?> Get(CancellationToken ct, SortBy? st = null, Pagination? pg = null);
        Task<CinemaResponse?> GetById(string id, CancellationToken ct);
        Task<Page<CinemasResponse>?> GetByIds(string[] ids, CancellationToken ct, Pagination? pg = null);
        Task<IEnumerable<SearchResponse>?> GetBySearch(string search, CancellationToken ct, Pagination? pg = null);
        Task<Page<CinemasResponse>?> GetByYear(int year, CancellationToken ct, SortBy? st = null, Pagination? pg = null);
        Task<Page<CinemasResponse>?> GetByGenre(Genre genre, CancellationToken ct, SortBy? st = null, Pagination? pg = null);
        Task<Page<CinemasResponse>?> GetByLanguage(Language language, CancellationToken ct, SortBy? st = null, Pagination? pg = null);
        Task<Page<CinemasResponse>?> GetByStudioId(string studioId, CancellationToken ct, SortBy? st = null, Pagination? pg = null);

        /******/

        /* Post Requests For Cinema */

        Task<CinemaResponse?> Create(CreateCinemaRequest cinema, CancellationToken ct);
        Task<ProductionStudioResponse?> CreateProductionStudioFor(string cinemaId, CreateProductionStudioRequest studio, CancellationToken ct);
        Task<StarringResponse?> CreateStarringFor(string cinemaId, CreateStarringRequest starring, CancellationToken ct);

        /******/

        /* Put Requests For Cinema */

        Task<CinemaResponse?> Update(string id, UpdateCinemaRequest cinema, CancellationToken ct);
        Task<UpdatePictureResponse?> UpdatePhoto(string cinemaId, UpdatePictureRequest picture, CancellationToken ct);
        Task<UpdateRatingResponse?> UpdateRating(string cinemaId, UpdateRatingRequest rating, CancellationToken ct);
        Task<ProductionStudioResponse?> UpdateProductionStudio(string studioId, UpdateProductionStudioRequest studio, CancellationToken ct);
        Task<StarringResponse?> UpdateStarring(string? cinemaId, string starringId, UpdateStarringRequest starring, CancellationToken ct);

        /******/

        /* Delete Requests For Cinema */

        Task<bool> Delete(string id, CancellationToken ct);
        Task<bool> DeleteProductionStudio(string? cinemaId, string studioId, CancellationToken ct);
        Task<bool> DeleteStarring(string? cinemaId, string starringId, CancellationToken ct);

        /******/
    }
}
