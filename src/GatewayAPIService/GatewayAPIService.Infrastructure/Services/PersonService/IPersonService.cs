
using Shared.CinemaDataService.Models.PersonDTO;
using Shared.CinemaDataService.Models.Flags;
using Shared.CinemaDataService.Models.RecordDTO;
using Shared.CinemaDataService.Models.SharedDTO;

namespace GatewayAPIService.Infrastructure.Services.PersonService
{
    public interface IPersonService
    {
        /* Get Requests For Person */
        Task<Page<PersonsResponse>> Get(SortBy? st = null, Pagination? pg = null);
        Task<PersonResponse> GetById(string id);
        Task<IEnumerable<SearchResponse>> GetBySearch(string search, SortBy? st = null, Pagination? pg = null);
        Task<Page<PersonsResponse>> GetByCountry(Country country, SortBy? st = null, Pagination? pg = null);
        Task<Page<PersonsResponse>> GetByJobs(Job genre, SortBy? st = null, Pagination? pg = null);

        /******/

        /* Post Requests For Person */

        Task<PersonResponse> Create(CreatePersonRequest person);
        Task<ProductionStudioResponse> CreateFilmographyFor(string personId, CreateFilmographyRequest studio);

        /******/

        /* Put Requests For Person */

        Task<PersonResponse> Update(string id, UpdatePersonRequest person);
        Task<UpdatePictureResponse> UpdatePhoto(string personId, UpdatePictureRequest picture);
        Task<FilmographyResponse> UpdateFilmography(string? personId, UpdateFilmographyRequest filmography);

        /******/

        /* Delete Requests For Person */

        Task Delete(string id);
        Task DeleteFilmography(string? personId, string filmographyId);

        /******/
    }
}
