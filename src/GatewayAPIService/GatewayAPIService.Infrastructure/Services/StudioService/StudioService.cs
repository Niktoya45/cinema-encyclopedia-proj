
using Shared.CinemaDataService.Models.Flags;
using Shared.CinemaDataService.Models.StudioDTO;
using Shared.CinemaDataService.Models.RecordDTO;
using Shared.CinemaDataService.Models.SharedDTO;
using System.Net.Http.Json;

namespace GatewayAPIService.Infrastructure.Services.StudioService
{
    public class StudioService:IStudioService
    {
        const string Url = "/api/studios";

        HttpClient _httpClient;

        string SortPaginate(SortBy? st, Pagination? pg) => (st == null ? "" : $"order={st.Order}&field={st.Field}&")
                                                          + (pg == null ? "" : $"skip={pg.Skip}&take={pg.Take}");
        public StudioService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /* Get Requests For Studio */
        public async Task<Page<StudiosResponse>?> Get(CancellationToken ct, SortBy? st = null, Pagination? pg = null)
        {
            return await _httpClient.GetFromJsonAsync<Page<StudiosResponse>>(Url + "?" + SortPaginate(st, pg), ct);
        }
        public async Task<StudioResponse?> GetById(string id, CancellationToken ct)
        {
            return await _httpClient.GetFromJsonAsync<StudioResponse>(Url + $"/{id}");
        }
        public async Task<IEnumerable<SearchResponse>?> GetBySearch(string search, CancellationToken ct, Pagination? pg = null)
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<SearchResponse>>(Url + $"/search/{search}?" + SortPaginate(null, pg));
        }
        public async Task<Page<StudiosResponse>?> GetByYear(int year, CancellationToken ct, SortBy? st = null, Pagination? pg = null)
        {
            return await _httpClient.GetFromJsonAsync<Page<StudiosResponse>>(Url + $"/year/{year}?" + SortPaginate(st, pg));
        }
        public async Task<Page<StudiosResponse>?> GetByCountry(Country country, CancellationToken ct, SortBy? st = null, Pagination? pg = null)
        {
            return await _httpClient.GetFromJsonAsync<Page<StudiosResponse>>(Url + $"/country/{country}?" + SortPaginate(st, pg));
        }

        /******/

        /* Post Requests For Studio */

        public async Task<StudioResponse?> Create(CreateStudioRequest studio, CancellationToken ct)
        {
            var response = await _httpClient.PostAsJsonAsync<CreateStudioRequest>(Url, studio, ct);

            return await response.Content.ReadFromJsonAsync<StudioResponse>();
        }
        public async Task<FilmographyResponse?> CreateFilmographyFor(string studioId, CreateFilmographyRequest filmography, CancellationToken ct)
        {
            var response = await _httpClient.PostAsJsonAsync<CreateFilmographyRequest>(Url + $"/{studioId}/filmography", filmography, ct);

            return await response.Content.ReadFromJsonAsync<FilmographyResponse>();
        }

        /******/

        /* Put Requests For Studio */

        public async Task<StudioResponse?> Update(string id, UpdateStudioRequest studio, CancellationToken ct)
        {
            var response = await _httpClient.PutAsJsonAsync<UpdateStudioRequest>(Url + $"/{id}", studio, ct);

            return await response.Content.ReadFromJsonAsync<StudioResponse>();
        }
        public async Task<UpdatePictureResponse?> UpdatePhoto(string studioId, UpdatePictureRequest picture, CancellationToken ct)
        {
            var response = await _httpClient.PutAsJsonAsync<UpdatePictureRequest>(Url + $"/{studioId}/picture", picture, ct);

            return await response.Content.ReadFromJsonAsync<UpdatePictureResponse>();
        }
        public async Task<FilmographyResponse?> UpdateFilmography(string? studioId, string filmographyId, UpdateFilmographyRequest filmography, CancellationToken ct)
        {
            var response = await _httpClient.PutAsJsonAsync<UpdateFilmographyRequest>(Url + $"/{studioId}/filmography/{filmographyId}", filmography, ct);

            return await response.Content.ReadFromJsonAsync<FilmographyResponse>();
        }

        /******/

        /* Delete Requests For Studio */

        public async Task<bool> Delete(string id, CancellationToken ct)
        {
            var response = await _httpClient.DeleteAsync(Url + $"/{id}", ct);

            return response.IsSuccessStatusCode;
        }
        public async Task<bool> DeleteFilmography(string? studioId, string filmographyId, CancellationToken ct)
        {
            var response = await _httpClient.DeleteAsync(Url + (studioId == null ? "" : $"/{studioId}") + $"/filmography/{filmographyId}", ct);

            return response.IsSuccessStatusCode;
        }

        /******/

    }
}
