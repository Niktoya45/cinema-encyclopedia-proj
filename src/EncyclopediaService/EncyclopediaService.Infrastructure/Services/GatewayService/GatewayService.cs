
using EncyclopediaService.Infrastructure.Extensions;
using Shared.CinemaDataService.Models.CinemaDTO;
using Shared.CinemaDataService.Models.Flags;
using Shared.CinemaDataService.Models.PersonDTO;
using Shared.CinemaDataService.Models.RecordDTO;
using Shared.CinemaDataService.Models.SharedDTO;
using Shared.CinemaDataService.Models.StudioDTO;
using Shared.ImageService.Models.ImageDTO;
using Shared.UserDataService.Models.LabeledDTO;
using Shared.UserDataService.Models.RatingDTO;
using Shared.UserDataService.Models.UserDTO;
using Shared.UserDataService.Models.Flags;
using System.Net.Http.Json;

namespace EncyclopediaService.Infrastructure.Services.GatewayService
{
    public class GatewayService:IGatewayService
    {
        const string cinemasUri = "/api/cinemas";
        const string personsUri = "/api/persons";
        const string studiosUri = "/api/studios";
        const string usersUri   = "/api/users";

        string sort_paginate(SortBy? st, Pagination? pg) => (st == null ? "" : $"order={st.Order}&field={st.Field}&")
                                                          + (pg == null ? "" : $"skip={pg.Skip}&take={pg.Take}");

        HttpClient _httpClient;

        public GatewayService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // ***    CINEMA API   *** //
        // *********************** //

        /* GET Requests For Cinema */
        public async Task<Page<CinemasResponse>?> GetCinemas(CancellationToken ct, SortBy? st = null, Pagination? pg = null)
        { 
            var response = await _httpClient.GetAsync(cinemasUri + "?" + sort_paginate(st, pg), ct);

            return await response.HandleResponse<Page<CinemasResponse>?>();
        }
        public async Task<Page<CinemasResponse>?> GetCinemasByIds(string[] ids, CancellationToken ct, SortBy? st = null)
        {
            var response = await _httpClient.PostAsJsonAsync(cinemasUri + "/indexes?" + sort_paginate(st, null), ids, ct);

            return await response.HandleResponse<Page<CinemasResponse>>();
        }
        public async Task<CinemaResponse?> GetCinemaById(string id, CancellationToken ct)
        {
            var response = await _httpClient.GetAsync(cinemasUri + $"/{id}", ct);

            return await response.HandleResponse<CinemaResponse>();
        }
        public async Task<IEnumerable<SearchResponse>?> GetCinemasBySearch(string search, CancellationToken ct, Pagination? pg = null)
        {
            var response = await _httpClient.GetAsync(cinemasUri + $"/search/{search}?" + sort_paginate(null, pg), ct);

            return await response.HandleResponse<IEnumerable<SearchResponse>>();
        }
        public async Task<Page<CinemasResponse>?> GetCinemasBySearchPage(string search, CancellationToken ct, Pagination? pg = null)
        {
            var response = await _httpClient.GetAsync(cinemasUri + $"/search-page/{search}?" + sort_paginate(null, pg), ct);

            return await response.HandleResponse<Page<CinemasResponse>>();
        }
        public async Task<Page<CinemasResponse>?> GetCinemasByYear(int year, CancellationToken ct, SortBy? st = null, Pagination? pg = null)
        {
            var response = await _httpClient.GetAsync(cinemasUri + $"/year/{year}?" + sort_paginate(st, pg), ct);

            return await response.HandleResponse<Page<CinemasResponse>>();
        }
        public async Task<Page<CinemasResponse>?> GetCinemasByYearSpans(int[] yearsLower, int yearSpan, CancellationToken ct, SortBy? st = null, Pagination? pg = null)
        {
            var response = await _httpClient.GetAsync(cinemasUri + $"/year" + $"?d={yearSpan}{yearsLower.Aggregate<int, string>("", (acc, y) => acc + $"&lys={y}")}&" + sort_paginate(st, pg), ct);

            return await response.HandleResponse<Page<CinemasResponse>>();
        }
        public async Task<Page<CinemasResponse>?> GetCinemasByGenre(Genre genre, CancellationToken ct, SortBy? st = null, Pagination? pg = null)
        {
            var response = await _httpClient.GetAsync(cinemasUri + $"/genres/{genre}?" + sort_paginate(st, pg), ct);

            return await response.HandleResponse<Page<CinemasResponse>>();
        }
        public async Task<Page<CinemasResponse>?> GetCinemasByLanguage(Language language, CancellationToken ct, SortBy? st = null, Pagination? pg = null)
        {
            var response = await _httpClient.GetAsync(cinemasUri + $"/language/{language}?" + sort_paginate(st, pg), ct);

            return await response.HandleResponse<Page<CinemasResponse>>();
        }
        public async Task<Page<CinemasResponse>?> GetCinemasByStudioId(string studioId, CancellationToken ct, SortBy? st = null, Pagination? pg = null)
        {
            var response = await _httpClient.GetAsync(cinemasUri + $"/studio/{studioId}?" + sort_paginate(st, pg), ct);

            return await response.HandleResponse<Page<CinemasResponse>>();
        }

        /******/

        /* POST Requests For Cinema */

        public async Task<CinemaResponse?> CreateCinema(CreateCinemaRequest cinema, CancellationToken ct)
        {
            var response = await _httpClient.PostAsJsonAsync(cinemasUri, cinema, ct);

            return await response.HandleResponse<CinemaResponse>();
        }
        public async Task<ProductionStudioResponse?> CreateCinemaProductionStudioFor(string cinemaId, CreateProductionStudioRequest studio, CancellationToken ct)
        {
            var response = await _httpClient.PostAsJsonAsync(cinemasUri + $"/{cinemaId}/production-studios/create", studio, ct);

            return await response.HandleResponse<ProductionStudioResponse>();
        }
        public async Task<StarringResponse?> CreateCinemaStarringFor(string cinemaId, CreateStarringRequest starring, CancellationToken ct)
        {
            var response = await _httpClient.PostAsJsonAsync(cinemasUri + $"/{cinemaId}/starrings/create", starring, ct);

            return await response.HandleResponse<StarringResponse>();
        }

        /******/

        /* PUT Requests For Cinema */

        public async Task<CinemaResponse?> UpdateCinema(string id, UpdateCinemaRequest cinema, CancellationToken ct)
        {
            var response = await _httpClient.PutAsJsonAsync(cinemasUri + $"/{id}", cinema, ct);

            return await response.HandleResponse<CinemaResponse>();
        }
        public async Task<CinemaResponse?> UpdateCinemaMain(string id, UpdateCinemaRequest cinema, CancellationToken ct)
        {
            var response = await _httpClient.PutAsJsonAsync(cinemasUri + $"/{id}/main", cinema, ct);

            return await response.HandleResponse<CinemaResponse>();
        }
        public async Task<UpdatePictureResponse?> UpdateCinemaPhoto(string cinemaId, ReplaceImageRequest picture, CancellationToken ct)
        {
            var response = await _httpClient.PutAsJsonAsync(cinemasUri + $"/{cinemaId}/picture/update", picture, ct);

            return await response.HandleResponse<UpdatePictureResponse>();
        }
        public async Task<StarringResponse?> UpdateCinemaStarring(string? cinemaId, string starringId, UpdateStarringRequest starring, CancellationToken ct)
        {
            var response = await _httpClient.PutAsJsonAsync(cinemasUri + $"/{cinemaId}/starrings/update/{starringId}", starring, ct);

            return await response.HandleResponse<StarringResponse>();
        }

        /******/

        /* DELETE Requests For Cinema */

        public async Task<bool> DeleteCinema(string id, CancellationToken ct)
        {
            var response = await _httpClient.DeleteAsync(cinemasUri + $"/{id}", ct);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteCinemaProductionStudio(string? cinemaId, string studioId, CancellationToken ct)
        {
            var response = await _httpClient.DeleteAsync(cinemasUri + $"/{cinemaId}/production-studios/delete/{studioId}", ct);

            return response.IsSuccessStatusCode;
        }
        public async Task<bool> DeleteCinemaStarring(string? cinemaId, string starringId, CancellationToken ct)
        {
            var response = await _httpClient.DeleteAsync(cinemasUri + $"/{cinemaId}/starrings/delete/{starringId}", ct);

            return response.IsSuccessStatusCode;
        }

        /******/

        // *********************** //
        // ***    CINEMA API   *** //


        // ***    PERSON API   *** //
        // *********************** //

        /* GET Requests For Person */
        public async Task<Page<PersonsResponse>?> GetPersons(CancellationToken ct, SortBy? st = null, Pagination? pg = null)
        {
            var response = await _httpClient.GetAsync(personsUri + "?" + sort_paginate(st, pg), ct);

            return await response.HandleResponse<Page<PersonsResponse>>();
        }
        public async Task<Page<PersonsResponse>?> GetPersonsByIds(string[] ids, CancellationToken ct, SortBy? st = null)
        {
            var response = await _httpClient.PostAsJsonAsync(personsUri + "/indexes?" + sort_paginate(st, null), ids, ct);

            return await response.HandleResponse<Page<PersonsResponse>>();
        }
        public async Task<PersonResponse?> GetPersonById(string id, CancellationToken ct)
        {
            var response = await _httpClient.GetAsync(personsUri + $"/{id}", ct);

            return await response.HandleResponse<PersonResponse>();
        }
        public async Task<IEnumerable<SearchResponse>?> GetPersonsBySearch(string search, CancellationToken ct, Pagination? pg = null)
        {
            var response = await _httpClient.GetAsync(personsUri + $"/search/{search}?" + sort_paginate(null, pg), ct);

            return await response.HandleResponse<IEnumerable<SearchResponse>>();
        }
        public async Task<Page<PersonsResponse>?> GetPersonsBySearchPage(string search, CancellationToken ct, Pagination? pg = null)
        {
            var response = await _httpClient.GetAsync(personsUri + $"/search-page/{search}?" + sort_paginate(null, pg), ct);

            return await response.HandleResponse<Page<PersonsResponse>>();
        }
        public async Task<Page<PersonsResponse>?> GetPersonsByCountry(Country country, CancellationToken ct, SortBy? st = null, Pagination? pg = null)
        {
            var response = await _httpClient.GetAsync(personsUri + $"/country/{country}?" + sort_paginate(st, pg), ct);

            return await response.HandleResponse<Page<PersonsResponse>>();
        }
        public async Task<Page<PersonsResponse>?> GetPersonsByJobs(Job jobs, CancellationToken ct, SortBy? st = null, Pagination? pg = null)
        {
            var response = await _httpClient.GetAsync(personsUri + $"/jobs/{jobs}?" + sort_paginate(st, pg), ct);

            return await response.HandleResponse<Page<PersonsResponse>>();
        }

        /******/

        /* POST Requests For Person */

        public async Task<PersonResponse?> CreatePerson(CreatePersonRequest person, CancellationToken ct)
        {
            var response = await _httpClient.PostAsJsonAsync(personsUri, person, ct);

            return await response.HandleResponse<PersonResponse>();
        }
        public async Task<FilmographyResponse?> CreatePersonFilmographyFor(string personId, CreateFilmographyRequest filmography, CancellationToken ct)
        {
            var response = await _httpClient.PostAsJsonAsync(personsUri + $"/{personId}/filmography/create", filmography, ct);

            return await response.HandleResponse<FilmographyResponse>();
        }

        /******/

        /* PUT Requests For Person */

        public async Task<PersonResponse?> UpdatePerson(string id, UpdatePersonRequest person, CancellationToken ct)
        {
            var response = await _httpClient.PutAsJsonAsync(studiosUri + $"/{id}", person, ct);

            return await response.HandleResponse<PersonResponse>();
        }

        public async Task<PersonResponse?> UpdatePersonMain(string id, UpdatePersonRequest person, CancellationToken ct)
        {
            var response = await _httpClient.PutAsJsonAsync(studiosUri + $"/{id}/main", person, ct);

            return await response.HandleResponse<PersonResponse>();
        }
        public async Task<UpdatePictureResponse?> UpdatePersonPhoto(string personId, ReplaceImageRequest picture, CancellationToken ct)
        {
            var response = await _httpClient.PutAsJsonAsync(studiosUri + $"/{personId}/picture/update", picture, ct);

            return await response.HandleResponse<UpdatePictureResponse>();
        }

        /******/

        /* DELETE Requests For Person */

        public async Task<bool> DeletePerson(string id, CancellationToken ct)
        {
            var response = await _httpClient.DeleteAsync(personsUri + $"/{id}", ct);

            return response.IsSuccessStatusCode;
        }
        public async Task<bool> DeletePersonFilmography(string? personId, string filmographyId, CancellationToken ct)
        {
            var response = await _httpClient.DeleteAsync(personsUri + $"/{personId}/filmography/delete/{filmographyId}", ct);

            return response.IsSuccessStatusCode;
        }

        /******/

        // *********************** //
        // ***    PERSON API   *** //


        // ***    STUDIO API   *** //
        // *********************** //

        /* GET Requests For Studio */
        public async Task<Page<StudiosResponse>?> GetStudios(CancellationToken ct, SortBy? st = null, Pagination? pg = null)
        {
            var response = await _httpClient.GetAsync(studiosUri +  "?" + sort_paginate(st, pg), ct);

            return await response.HandleResponse<Page<StudiosResponse>>();
        }
        public async Task<Page<StudiosResponse>?> GetStudiosByIds(string[] ids, CancellationToken ct, SortBy? st = null)
        {
            var response = await _httpClient.PostAsJsonAsync(studiosUri +  "/indexes?" + sort_paginate(st, null), ids, ct);

            return await response.HandleResponse<Page<StudiosResponse>>();
        }
        public async Task<StudioResponse?> GetStudioById(string id, CancellationToken ct)
        {
            var response = await _httpClient.GetAsync(studiosUri + $"/{id}", ct);

            return await response.HandleResponse<StudioResponse>();
        }
        public async Task<IEnumerable<SearchResponse>?> GetStudiosBySearch(string search, CancellationToken ct, Pagination? pg = null)
        {
            var response = await _httpClient.GetAsync(studiosUri + $"/search/{search}?" + sort_paginate(null, pg), ct);

            return await response.HandleResponse<IEnumerable<SearchResponse>>();
        }
        public async Task<Page<StudiosResponse>?> GetStudiosBySearchPage(string search, CancellationToken ct, Pagination? pg = null)
        {
            var response = await _httpClient.GetAsync(studiosUri + $"/search-page/{search}?" + sort_paginate(null, pg), ct);

            return await response.HandleResponse<Page<StudiosResponse>>();
        }
        public async Task<Page<StudiosResponse>?> GetStudiosByYear(int year, CancellationToken ct, SortBy? st = null, Pagination? pg = null)
        {
            var response = await _httpClient.GetAsync(studiosUri + $"/year/{year}?" + sort_paginate(st, pg), ct);

            return await response.HandleResponse<Page<StudiosResponse>>();
        }
        public async Task<Page<StudiosResponse>?> GetStudiosByYearSpans(int[] yearsLower, int yearSpan, CancellationToken ct, SortBy? st = null, Pagination? pg = null)
        {
            var response = await _httpClient.GetAsync(studiosUri + $"/year" + $"?d={yearSpan}{yearsLower.Aggregate<int, string>("", (acc, y) => acc + $"&lys={y}")}&" + sort_paginate(st, pg), ct);

            return await response.HandleResponse<Page<StudiosResponse>>();
        }
        public async Task<Page<StudiosResponse>?> GetStudiosByCountry(Country country, CancellationToken ct, SortBy? st = null, Pagination? pg = null)
        {
            var response = await _httpClient.GetAsync(studiosUri + $"/country/{country}?" + sort_paginate(st, pg), ct);

            return await response.HandleResponse<Page<StudiosResponse>>();
        }

        /******/

        /* POST Requests For Studio */

        public async Task<StudioResponse?> CreateStudio(CreateStudioRequest studio, CancellationToken ct)
        {
            var response = await _httpClient.PostAsJsonAsync(studiosUri, studio, ct);

            return await response.HandleResponse<StudioResponse>();
        }
        public async Task<FilmographyResponse?> CreateStudioFilmographyFor(string studioId, CreateFilmographyRequest filmography, CancellationToken ct)
        {
            var response = await _httpClient.PostAsJsonAsync(studiosUri + $"/{studioId}/filmography/create", filmography, ct);

            return await response.HandleResponse<FilmographyResponse>();
        }

        /******/

        /* PUT Requests For Studio */

        public async Task<StudioResponse?> UpdateStudio(string id, UpdateStudioRequest studio, CancellationToken ct)
        {
            var response = await _httpClient.PutAsJsonAsync(studiosUri + $"/{id}", studio, ct);

            return await response.HandleResponse<StudioResponse>();
        }
        public async Task<StudioResponse?> UpdateStudioMain(string id, UpdateStudioRequest studio, CancellationToken ct)
        {
            var response = await _httpClient.PutAsJsonAsync(studiosUri + $"/{id}/main", studio, ct);

            return await response.HandleResponse<StudioResponse>();
        }
        public async Task<UpdatePictureResponse?> UpdateStudioPhoto(string studioId, ReplaceImageRequest picture, CancellationToken ct)
        {
            var response = await _httpClient.PutAsJsonAsync(studiosUri + $"/{studioId}/picture/update", picture, ct);

            return await response.HandleResponse<UpdatePictureResponse>();
        }

        /******/

        /* DELETE Requests For Studio */

        public async Task<bool> DeleteStudio(string id, CancellationToken ct)
        {
            var response = await _httpClient.DeleteAsync(studiosUri + $"/{id}", ct);

            return response.IsSuccessStatusCode;
        }
        public async Task<bool> DeleteStudioFilmography(string studioId, string filmographyId, CancellationToken ct)
        {
            var response = await _httpClient.DeleteAsync(studiosUri + $"/{studioId}/filmography/delete/{filmographyId}", ct);

            return response.IsSuccessStatusCode;
        }

        /******/

        // *********************** //
        // ***    STUDIO API   *** //


        // ***    USER INTERACTION API   *** //
        // *********************** //

        /* User GET Requests */
        public async Task<UserResponse?> GetUser(string id, CancellationToken ct)
        {
            var response = await _httpClient.GetAsync(usersUri + $"/{id}", ct);

            return await response.HandleResponse<UserResponse>();
        }
        public async Task<LabeledCinemasResponse<CinemasResponse>?> GetUserLabeled(string userId, Label? label, CancellationToken ct)
        {
            var response = await _httpClient.GetAsync(usersUri + $"/{userId}/label/{label}", ct);

            return await response.HandleResponse<LabeledCinemasResponse<CinemasResponse>>();
        }
        public async Task<LabeledCinemasResponse<CinemasResponse>?> GetUserLabelFor(string userId, string cinemaId, CancellationToken ct)
        {
            var response = await _httpClient.GetAsync(usersUri + $"/label?cinemaId={cinemaId}", ct);

            return await response.HandleResponse<LabeledCinemasResponse<CinemasResponse>>();
        }
        public async Task<RatingResponse?> GetUserRatingFor(string userId, string cinemaId, CancellationToken ct)
        {
            var response = await _httpClient.GetAsync(usersUri + $"/{userId}/rating/{cinemaId}", ct);

            return await response.HandleResponse<RatingResponse>();
        }

        /******/

        /* User POST Requests */
        public async Task<UserResponse?> CreateUser(CreateUserRequest user, CancellationToken ct)
        {
            var response = await _httpClient.PostAsJsonAsync(usersUri, user, ct);

            return await response.HandleResponse<UserResponse>();
        }
        public async Task<LabeledResponse?> CreateForLabeledList(string userId, CreateLabeledRequest labeled, CancellationToken ct)
        {
            var response = await _httpClient.PostAsJsonAsync(usersUri + $"/{userId}/label", labeled, ct);

            return await response.HandleResponse<LabeledResponse>();
        }
        /******/

        /* User PUT Requests */
        public async Task<UserResponse?> UpdateUser(string id, UpdateUserRequest user, CancellationToken ct)
        {
            var response = await _httpClient.PutAsJsonAsync(usersUri + $"/{id}", user, ct);

            return await response.HandleResponse<UserResponse>();
        }
        public async Task<RatingResponse?> UpdateRatingList(string userId, UpdateRatingRequest rating, CancellationToken ct)
        {
            var response = await _httpClient.PutAsJsonAsync(usersUri + $"/{userId}/rating", rating, ct);

            return await response.HandleResponse<RatingResponse>();
        }


        /******/

        /* User DELETE Requests */
        public async Task<bool> DeleteUser(string id, CancellationToken ct)
        {
            var response = await _httpClient.DeleteAsync(usersUri + $"/{id}", ct);

            return response.IsSuccessStatusCode;
        }
        public async Task<LabeledResponse?> DeleteFromLabeledList(string userId, string? cinemaId, Label label, CancellationToken ct)
        {
            var response = await _httpClient.DeleteAsync(usersUri + $"/{userId}/label/{cinemaId}" + (label == Label.None ? "" : $"/{label}"), ct);

            return await response.HandleResponse<LabeledResponse>();
        }

        /******/

        // *********************** //
        // ***    USER INTERACTION API   *** //
    }
}
