

using Shared.CinemaDataService.Models.Flags;
using Shared.CinemaDataService.Models.StudioDTO;
using Shared.CinemaDataService.Models.RecordDTO;
using Shared.CinemaDataService.Models.SharedDTO;

namespace GatewayAPIService.Infrastructure.Services.StudioService
{
    internal interface IStudioService
    {
        /* Get Requests For Studio */
        Task<Page<StudiosResponse>> Get(SortBy? st = null, Pagination? pg = null);
        Task<StudioResponse> GetById(string id);
        Task<IEnumerable<SearchResponse>> GetBySearch(string search, SortBy? st = null, Pagination? pg = null);
        Task<Page<StudiosResponse>> GetByYear(int year, SortBy? st = null, Pagination? pg = null);
        Task<Page<StudiosResponse>> GetByCountry(Country country, SortBy? st = null, Pagination? pg = null);

        /******/

        /* Post Requests For Studio */

        Task<StudioResponse> Create(CreateStudioRequest studio);
        Task<ProductionStudioResponse> CreateFilmographyFor(string studioId, CreateFilmographyRequest studio);

        /******/

        /* Put Requests For Studio */

        Task<StudioResponse> Update(string id, UpdateStudioRequest studio);
        Task<UpdatePictureResponse> UpdatePhoto(string studioId, UpdatePictureRequest picture);
        Task<FilmographyResponse> UpdateFilmography(string? studioId, UpdateFilmographyRequest filmography);

        /******/

        /* Delete Requests For Studio */

        Task Delete(string id);
        Task DeleteFilmography(string? studioId, string filmographyId);

        /******/
    }
}
