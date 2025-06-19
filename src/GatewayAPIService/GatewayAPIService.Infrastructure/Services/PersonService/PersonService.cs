
using Shared.CinemaDataService.Models.Flags;
using Shared.CinemaDataService.Models.PersonDTO;
using Shared.CinemaDataService.Models.RecordDTO;
using Shared.CinemaDataService.Models.SharedDTO;
using GatewayAPIService.Infrastructure.Extensions;
using System.Net.Http.Json;

namespace GatewayAPIService.Infrastructure.Services.PersonService
{
    public class PersonService:IPersonService
    {
        const string personsUri = "/api/persons";

        HttpClient _httpClient;

        string sort_paginate(SortBy? st, Pagination? pg) => (st == null ? "" : $"order={st.Order}&field={st.Field}&")
                                                          + (pg == null ? "" : $"skip={pg.Skip}&take={pg.Take}");
        public PersonService(HttpClient httpClient) 
        {
            _httpClient = httpClient;
        }

        /* Get Requests For Person */
        public async Task<Page<PersonsResponse>?> Get(CancellationToken ct, SortBy? st = null, Pagination? pg = null) 
        {
            var response = await _httpClient.GetAsync(personsUri + "?" + sort_paginate(st, pg), ct);

            return await response.HandleResponse<Page<PersonsResponse>>(ct);
        }
        public async Task<PersonResponse?> GetById(string id, CancellationToken ct) 
        {
            var response = await _httpClient.GetAsync(personsUri + $"/{id}");

            return await response.HandleResponse<PersonResponse>(ct);
        }
        public async Task<FilmographyResponse?> GetFilmographyById(string personId, string filmographyId, CancellationToken ct)
        {
            var response = await _httpClient.GetAsync(personsUri + $"{personId}/filmography/{filmographyId}", ct);

            return await response.HandleResponse<FilmographyResponse>(ct);
        }
        public async Task<IEnumerable<SearchResponse>?> GetBySearch(string search, CancellationToken ct, Pagination? pg = null)
        {
            var response = await _httpClient.GetAsync(personsUri+$"/search/{search}?" + sort_paginate(null, pg), ct);      
            
            return await response.HandleResponse<IEnumerable<SearchResponse>>(ct);
        }
        public async Task<Page<PersonsResponse>?> GetByCountry(Country country, CancellationToken ct, SortBy? st = null, Pagination? pg = null)
        {
            var response = await _httpClient.GetAsync(personsUri+$"/country/{country}?" + sort_paginate(st, pg), ct);

            return await response.HandleResponse<Page<PersonsResponse>>(ct);
        }
        public async Task<Page<PersonsResponse>?> GetByJobs(Job jobs, CancellationToken ct, SortBy? st = null, Pagination? pg = null)
        {
            var response = await _httpClient.GetAsync(personsUri + $"/jobs/{jobs}?" + sort_paginate(st, pg), ct);

            return await response.HandleResponse<Page<PersonsResponse>>(ct);
        }

        /******/

        /* Post Requests For Person */

        public async Task<PersonResponse?> Create(CreatePersonRequest person, CancellationToken ct)
        {
            var response = await _httpClient.PostAsJsonAsync<CreatePersonRequest>(personsUri, person, ct);

            return await response.HandleResponse<PersonResponse>(ct);
        }
        public async Task<FilmographyResponse?> CreateFilmographyFor(string personId, CreateFilmographyRequest filmography, CancellationToken ct)
        {
            var response = await _httpClient.PostAsJsonAsync<CreateFilmographyRequest>(personsUri+$"/{personId}/filmography", filmography, ct);

            return await response.HandleResponse<FilmographyResponse>(ct);
        }

        /******/

        /* Put Requests For Person */

        public async Task<PersonResponse?> Update(string id, UpdatePersonRequest person, CancellationToken ct)
        {
            var response = await _httpClient.PutAsJsonAsync<UpdatePersonRequest>(personsUri +$"/{id}", person, ct);

            return await response.HandleResponse<PersonResponse>(ct);
        }
        public async Task<UpdatePictureResponse?> UpdatePhoto(string personId, UpdatePictureRequest picture, CancellationToken ct)
        {
            var response = await _httpClient.PutAsJsonAsync<UpdatePictureRequest>(personsUri+$"/{personId}/picture", picture, ct);

            return await response.HandleResponse<UpdatePictureResponse>(ct);
        }
        public async Task<FilmographyResponse?> UpdateFilmography(string? personId, string filmographyId, UpdateFilmographyRequest filmography, CancellationToken ct)
        {
            var response = await _httpClient.PutAsJsonAsync<UpdateFilmographyRequest>(personsUri + (personId == null ? "" : $"/{personId}") + $"/filmography/{filmographyId}", filmography, ct);

            return await response.HandleResponse<FilmographyResponse>(ct);
        }

        /******/

        /* Delete Requests For Person */

        public async Task<bool> Delete(string id, CancellationToken ct)
        {
            var response = await _httpClient.DeleteAsync(personsUri+$"/{id}", ct);

            return response.IsSuccessStatusCode;
        }
        public async Task<bool> DeleteFilmography(string? personId, string filmographyId, CancellationToken ct)
        {
            var response = await _httpClient.DeleteAsync(personsUri + (personId == null ? "" : $"/{personId}") + $"/filmography/{filmographyId}", ct);

            return response.IsSuccessStatusCode;
        }

        /******/
    }
}
