
using System.Globalization;
using Shared.CinemaDataService.Models.Flags;
using Shared.CinemaDataService.Models.CinemaDTO;
using Shared.CinemaDataService.Models.PersonDTO;
using Shared.CinemaDataService.Models.StudioDTO;
using Shared.CinemaDataService.Models.RecordDTO;
using Shared.CinemaDataService.Models.SharedDTO;
using Shared.ImageService.Models.ImageDTO;
using Shared.ImageService.Models.Flags;

namespace EncyclopediaService.Infrastructure.Services.GatewayService
{
    public interface IGatewayService
    {

        // ***    CINEMA API   *** //
        // *********************** //

        /* GET Requests For Cinema */
        Task<Page<CinemasResponse>?> GetCinemas(CancellationToken ct, SortBy? st = null, Pagination? pg = null);
        Task<CinemaResponse?> GetCinemaById(string id, CancellationToken ct);
        Task<IEnumerable<SearchResponse>?> GetCinemasBySearch(string search, CancellationToken ct, Pagination? pg = null);
        Task<Page<CinemasResponse>?> GetCinemasByYear(int year, CancellationToken ct, SortBy? st = null, Pagination? pg = null);
        Task<Page<CinemasResponse>?> GetCinemasByGenre(Genre genre, CancellationToken ct, SortBy? st = null, Pagination? pg = null);
        Task<Page<CinemasResponse>?> GetCinemasByLanguage(Language language, CancellationToken ct, SortBy? st = null, Pagination? pg = null);
        Task<Page<CinemasResponse>?> GetCinemasByStudioId(string studioId, CancellationToken ct, SortBy? st = null, Pagination? pg = null);

        /******/

        /* POST Requests For Cinema */

        Task<CinemaResponse?> CreateCinema(CreateCinemaRequest cinema, CancellationToken ct);
        Task<ProductionStudioResponse?> CreateCinemaProductionStudioFor(string cinemaId, CreateProductionStudioRequest studio, CancellationToken ct);
        Task<StarringResponse?> CreateCinemaStarringFor(string cinemaId, CreateStarringRequest starring, CancellationToken ct);

        /******/

        /* PUT Requests For Cinema */

        Task<CinemaResponse?> UpdateCinema(string id, UpdateCinemaRequest cinema, CancellationToken ct);
        Task<UpdatePictureResponse?> UpdateCinemaPhoto(string cinemaId, ReplaceImageRequest picture, CancellationToken ct);
        Task<StarringResponse?> UpdateCinemaStarring(string? cinemaId, string starringId, UpdateStarringRequest starring, CancellationToken ct);

        /******/

        /* DELETE Requests For Cinema */

        Task<bool> DeleteCinema(string id, CancellationToken ct);
        Task<bool> DeleteCinemaProductionStudio(string? cinemaId, string studioId, CancellationToken ct);
        Task<bool> DeleteCinemaStarring(string cinemaId, string starringId, CancellationToken ct);

        /******/

        // *********************** //
        // ***    CINEMA API   *** //


        // ***    PERSON API   *** //
        // *********************** //

        /* GET Requests For Person */
        Task<Page<PersonsResponse>?> GetPersons(CancellationToken ct, SortBy? st = null, Pagination? pg = null);
        Task<PersonResponse?> GetPersonById(string id, CancellationToken ct);
        Task<IEnumerable<SearchResponse>?> GetPersonsBySearch(string search, CancellationToken ct, Pagination? pg = null);
        Task<Page<PersonsResponse>?> GetPersonsByCountry(Country country, CancellationToken ct, SortBy? st = null, Pagination? pg = null);
        Task<Page<PersonsResponse>?> GetPersonsByJobs(Job jobs, CancellationToken ct, SortBy? st = null, Pagination? pg = null);

        /******/

        /* POST Requests For Person */

        Task<PersonResponse?> CreatePerson(CreatePersonRequest person, CancellationToken ct);
        Task<FilmographyResponse?> CreatePersonFilmographyFor(string personId, CreateFilmographyRequest filmography, CancellationToken ct);

        /******/

        /* PUT Requests For Person */

        Task<PersonResponse?> UpdatePerson(string id, UpdatePersonRequest person, CancellationToken ct);
        Task<UpdatePictureResponse?> UpdatePersonPhoto(string personId, ReplaceImageRequest picture, CancellationToken ct);

        /******/

        /* DELETE Requests For Person */

        Task<bool> DeletePerson(string id, CancellationToken ct);
        Task<bool> DeletePersonFilmography(string personId, string filmographyId, CancellationToken ct);

        /******/

        // *********************** //
        // ***    PERSON API   *** //


        // ***    STUDIO API   *** //
        // *********************** //

        /* GET Requests For Studio */
        Task<Page<StudiosResponse>?> GetStudios(CancellationToken ct, SortBy? st = null, Pagination? pg = null);
        Task<StudioResponse?> GetStudioById(string id, CancellationToken ct);
        Task<IEnumerable<SearchResponse>?> GetStudiosBySearch(string search, CancellationToken ct, Pagination? pg = null);
        Task<Page<StudiosResponse>?> GetStudiosByYear(int year, CancellationToken ct, SortBy? st = null, Pagination? pg = null);
        Task<Page<StudiosResponse>?> GetStudiosByCountry(Country country, CancellationToken ct, SortBy? st = null, Pagination? pg = null);

        /******/

        /* POST Requests For Studio */

        Task<StudioResponse?> CreateStudio(CreateStudioRequest studio, CancellationToken ct);
        Task<FilmographyResponse?> CreateStudioFilmographyFor(string studioId, CreateFilmographyRequest filmography, CancellationToken ct);

        /******/

        /* PUT Requests For Studio */

        Task<StudioResponse?> UpdateStudio(string id, UpdateStudioRequest studio, CancellationToken ct);
        Task<UpdatePictureResponse?> UpdateStudioPhoto(string studioId, ReplaceImageRequest picture, CancellationToken ct);

        /******/

        /* DELETE Requests For Studio */

        Task<bool> DeleteStudio(string id, CancellationToken ct);
        Task<bool> DeleteStudioFilmography(string studioId, string filmographyId, CancellationToken ct);

        /******/

        // *********************** //
        // ***    STUDIO API   *** //


        // ***    USER INTERACTION API   *** //
        // *********************** //

        /* GET Requests For User */
        /******/

        /* POST Requests For User */
        /******/

        /* PUT Requests For User */
        /******/

        /* DELETE Requests For User */
        /******/

        // *********************** //
        // ***    USER INTERACTION API   *** //
    }
}
