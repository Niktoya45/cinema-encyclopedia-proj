
using Shared.CinemaDataService.Models.Flags;
using Shared.CinemaDataService.Models.PersonDTO;
using Shared.CinemaDataService.Models.RecordDTO;
using Shared.CinemaDataService.Models.SharedDTO;
using System;
using System.Net.Http.Json;

namespace GatewayAPIService.Infrastructure.Services.PersonService
{
    public class PersonService:IPersonService
    {
        const string Url = "/api/persons";

        HttpClient _httpClient;

        string SortPaginate(SortBy? st, Pagination? pg) => (st == null ? "" : $"order={st.Order}&field={st.Field}&")
                                                          + (pg == null ? "" : $"skip={pg.Skip}&take={pg.Take}");
        public PersonService(HttpClient httpClient) 
        {
            _httpClient = httpClient;
        }

        /* Get Requests For Person */
        public async Task<Page<PersonsResponse>?> Get(CancellationToken ct, SortBy? st = null, Pagination? pg = null) 
        {
            return await _httpClient.GetFromJsonAsync<Page<PersonsResponse>>(Url + "?" + SortPaginate(st, pg), ct);
        }
        public async Task<PersonResponse?> GetById(string id, CancellationToken ct) 
        {
            return await _httpClient.GetFromJsonAsync<PersonResponse>(Url + $"/{id}");
        }
        public async Task<IEnumerable<SearchResponse>?> GetBySearch(string search, CancellationToken ct, Pagination? pg = null)
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<SearchResponse>>(Url+$"/search/{search}?" + SortPaginate(null, pg));        
        }
        public async Task<Page<PersonsResponse>?> GetByCountry(Country country, CancellationToken ct, SortBy? st = null, Pagination? pg = null)
        {
            return await _httpClient.GetFromJsonAsync<Page<PersonsResponse>>(Url+$"/country/{country}?" + SortPaginate(st, pg));
        }
        public async Task<Page<PersonsResponse>?> GetByJobs(Job jobs, CancellationToken ct, SortBy? st = null, Pagination? pg = null)
        {
            return await _httpClient.GetFromJsonAsync<Page<PersonsResponse>>(Url + $"/jobs/{jobs}?" + SortPaginate(st, pg));
        }

        /******/

        /* Post Requests For Person */

        public async Task<PersonResponse?> Create(CreatePersonRequest person, CancellationToken ct)
        {
            var response = await _httpClient.PostAsJsonAsync<CreatePersonRequest>(Url, person, ct);

            return await response.Content.ReadFromJsonAsync<PersonResponse>();
        }
        public async Task<FilmographyResponse?> CreateFilmographyFor(string personId, CreateFilmographyRequest filmography, CancellationToken ct)
        {
            var response = await _httpClient.PostAsJsonAsync<CreateFilmographyRequest>(Url+$"/{personId}/filmography", filmography, ct);

            return await response.Content.ReadFromJsonAsync<FilmographyResponse>();
        }

        /******/

        /* Put Requests For Person */

        public async Task<PersonResponse?> Update(string id, UpdatePersonRequest person, CancellationToken ct)
        {
            var response = await _httpClient.PutAsJsonAsync<UpdatePersonRequest>(Url +$"/{id}", person, ct);

            return await response.Content.ReadFromJsonAsync<PersonResponse>();
        }
        public async Task<UpdatePictureResponse?> UpdatePhoto(string personId, UpdatePictureRequest picture, CancellationToken ct)
        {
            var response = await _httpClient.PutAsJsonAsync<UpdatePictureRequest>(Url+$"/{personId}/picture", picture, ct);

            return await response.Content.ReadFromJsonAsync<UpdatePictureResponse>();
        }
        public async Task<FilmographyResponse?> UpdateFilmography(string? personId, string filmographyId, UpdateFilmographyRequest filmography, CancellationToken ct)
        {
            var response = await _httpClient.PutAsJsonAsync<UpdateFilmographyRequest>(Url+$"/{personId}/filmography/{filmographyId}", filmography, ct);

            return await response.Content.ReadFromJsonAsync<FilmographyResponse>();
        }

        /******/

        /* Delete Requests For Person */

        public async Task<bool> Delete(string id, CancellationToken ct)
        {
            var response = await _httpClient.DeleteAsync(Url+$"/{id}", ct);

            return response.IsSuccessStatusCode;
        }
        public async Task<bool> DeleteFilmography(string? personId, string filmographyId, CancellationToken ct)
        {
            var response = await _httpClient.DeleteAsync(Url + (personId == null ? "" : $"/{personId}") + $"/filmography/{filmographyId}", ct);

            return response.IsSuccessStatusCode;
        }

        /******/
    }
}
