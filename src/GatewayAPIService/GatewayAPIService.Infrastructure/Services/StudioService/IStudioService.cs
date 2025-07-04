

using Shared.CinemaDataService.Models.Flags;
using Shared.CinemaDataService.Models.StudioDTO;
using Shared.CinemaDataService.Models.RecordDTO;
using Shared.CinemaDataService.Models.SharedDTO;

namespace GatewayAPIService.Infrastructure.Services.StudioService
{
    public interface IStudioService
    {
        /* Get Requests For Studio */
        Task<Page<StudiosResponse>?> Get(CancellationToken ct, SortBy? st = null, Pagination? pg = null);
        Task<Page<StudiosResponse>?> GetByIds(string[] ids, CancellationToken ct, SortBy? st = null);
        Task<StudioResponse?> GetById(string id, CancellationToken ct);
        Task<FilmographyResponse?> GetFilmographyById(string studioId, string filmographyId, CancellationToken ct);
        Task<IEnumerable<SearchResponse>?> GetBySearch(string search, CancellationToken ct, Pagination? pg = null);
        Task<Page<StudiosResponse>?> GetBySearchPage(string search, CancellationToken ct, Pagination? pg = null);
        Task<Page<StudiosResponse>?> GetByYear(int year, CancellationToken ct, SortBy? st = null, Pagination? pg = null);
        Task<Page<StudiosResponse>?> GetByYearSpans(int[] yearsLower, int yearSpan, CancellationToken ct, SortBy? st = null, Pagination? pg = null);
        Task<Page<StudiosResponse>?> GetByCountry(Country country, CancellationToken ct, SortBy? st = null, Pagination? pg = null);

        /******/

        /* Post Requests For Studio */

        Task<StudioResponse?> Create(CreateStudioRequest studio, CancellationToken ct);
        Task<FilmographyResponse?> CreateFilmographyFor(string studioId, CreateFilmographyRequest filmography, CancellationToken ct);

        /******/

        /* Put Requests For Studio */

        Task<StudioResponse?> Update(string id, UpdateStudioRequest studio, CancellationToken ct);
        Task<StudioResponse?> UpdateMain(string id, UpdateStudioRequest studio, CancellationToken ct);
        Task<UpdatePictureResponse?> UpdatePhoto(string studioId, UpdatePictureRequest picture, CancellationToken ct);
        Task<FilmographyResponse?> UpdateFilmography(string? studioId, string filmographyId, UpdateFilmographyRequest filmography, CancellationToken ct);

        /******/

        /* Delete Requests For Studio */

        Task<bool> Delete(string id, CancellationToken ct);
        Task<bool> DeleteFilmography(string? studioId, string filmographyId, CancellationToken ct);

        /******/
    }
}
