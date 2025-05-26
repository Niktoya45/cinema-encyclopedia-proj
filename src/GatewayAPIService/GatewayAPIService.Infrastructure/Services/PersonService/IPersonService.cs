
using Shared.CinemaDataService.Models.PersonDTO;
using Shared.CinemaDataService.Models.Flags;
using Shared.CinemaDataService.Models.RecordDTO;
using Shared.CinemaDataService.Models.SharedDTO;

namespace GatewayAPIService.Infrastructure.Services.PersonService
{
    public interface IPersonService
    {
        /* Get Requests For Person */
        Task<Page<PersonsResponse>?> Get(CancellationToken ct, SortBy? st = null, Pagination? pg = null);
        Task<PersonResponse?> GetById(string id, CancellationToken ct);
        Task<FilmographyResponse?> GetFilmographyById(string personId, string filmographyId, CancellationToken ct);
        Task<IEnumerable<SearchResponse>?> GetBySearch(string search, CancellationToken ct, Pagination? pg = null);
        Task<Page<PersonsResponse>?> GetByCountry(Country country, CancellationToken ct, SortBy? st = null, Pagination? pg = null);
        Task<Page<PersonsResponse>?> GetByJobs(Job jobs, CancellationToken ct, SortBy? st = null, Pagination? pg = null);

        /******/

        /* Post Requests For Person */

        Task<PersonResponse?> Create(CreatePersonRequest person, CancellationToken ct);
        Task<FilmographyResponse?> CreateFilmographyFor(string personId,  CreateFilmographyRequest filmography, CancellationToken ct);

        /******/

        /* Put Requests For Person */

        Task<PersonResponse?> Update(string id, UpdatePersonRequest person, CancellationToken ct);
        Task<UpdatePictureResponse?> UpdatePhoto(string personId, UpdatePictureRequest picture, CancellationToken ct);
        Task<FilmographyResponse?> UpdateFilmography(string? personId, string filmographyId, UpdateFilmographyRequest filmography, CancellationToken ct);

        /******/

        /* Delete Requests For Person */

        Task<bool> Delete(string id, CancellationToken ct);
        Task<bool> DeleteFilmography(string? personId, string filmographyId, CancellationToken ct);

        /******/
    }
}
