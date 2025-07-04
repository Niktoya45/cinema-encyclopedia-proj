
using GatewayAPIService.Infrastructure.Extensions;
using Shared.CinemaDataService.Models.CinemaDTO;
using Shared.CinemaDataService.Models.Flags;
using Shared.CinemaDataService.Models.RecordDTO;
using Shared.CinemaDataService.Models.SharedDTO;
using Shared.CinemaDataService.Models.StudioDTO;
using System.Net.Http.Json;

namespace GatewayAPIService.Infrastructure.Services.StudioService
{
    public class StudioService:IStudioService
    {
        const string studiosUri = "/api/studios";

        HttpClient _httpClient;

        string sort_paginate(SortBy? st, Pagination? pg) => (st == null ? "" : $"order={st.Order}&field={st.Field}&")
                                                          + (pg == null ? "" : $"skip={pg.Skip}&take={pg.Take}");
        public StudioService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /* Get Requests For Studio */
        public async Task<Page<StudiosResponse>?> Get(CancellationToken ct, SortBy? st = null, Pagination? pg = null)
        {
            var response = await _httpClient.GetAsync(studiosUri + "?" + sort_paginate(st, pg), ct);

            return await response.HandleResponse<Page<StudiosResponse>>(ct);
        }
        public async Task<Page<StudiosResponse>?> GetByIds(string[] ids, CancellationToken ct, SortBy? st = null)
        {
            var response = await _httpClient.PostAsJsonAsync(studiosUri + "/indexes?" + sort_paginate(st, null), ids, ct);

            return await response.HandleResponse<Page<StudiosResponse>>(ct);
        }
        public async Task<StudioResponse?> GetById(string id, CancellationToken ct)
        {
            var response = await _httpClient.GetAsync(studiosUri + $"/{id}", ct);

            return await response.HandleResponse<StudioResponse>(ct);
        }
        public async Task<FilmographyResponse?> GetFilmographyById(string studioId, string filmographyId, CancellationToken ct)
        {
            var response = await _httpClient.GetAsync(studiosUri + $"/{studioId}/filmography/{filmographyId}", ct);

            return await response.HandleResponse<FilmographyResponse>(ct);
        }
        public async Task<IEnumerable<SearchResponse>?> GetBySearch(string search, CancellationToken ct, Pagination? pg = null)
        {
            var response = await _httpClient.GetAsync(studiosUri + $"/search/{search}?" + sort_paginate(null, pg), ct);

            return await response.HandleResponse<IEnumerable<SearchResponse>>(ct);
        }
        public async Task<Page<StudiosResponse>?> GetBySearchPage(string search, CancellationToken ct, Pagination? pg = null)
        {
            var response = await _httpClient.GetAsync(studiosUri + $"/search-page/{search}?" + sort_paginate(null, pg), ct);

            return await response.HandleResponse<Page<StudiosResponse>>(ct);
        }
        public async Task<Page<StudiosResponse>?> GetByYear(int year, CancellationToken ct, SortBy? st = null, Pagination? pg = null)
        {
            var response = await _httpClient.GetAsync(studiosUri + $"/year/{year}?" + sort_paginate(st, pg), ct);

            return await response.HandleResponse<Page<StudiosResponse>>(ct);
        }
        public async Task<Page<StudiosResponse>?> GetByYearSpans(int[] yearsLower, int yearSpan, CancellationToken ct, SortBy? st = null, Pagination? pg = null)
        {
            var response = await _httpClient.GetAsync(studiosUri + $"/year" + $"?d={yearSpan}{yearsLower.Aggregate<int, string>("", (acc, y) => acc + $"&lys={y}")}&" + sort_paginate(st, pg), ct);

            return await response.HandleResponse<Page<StudiosResponse>>(ct);
        }
        public async Task<Page<StudiosResponse>?> GetByCountry(Country country, CancellationToken ct, SortBy? st = null, Pagination? pg = null)
        {
            var response = await _httpClient.GetAsync(studiosUri + $"/country/{country}?" + sort_paginate(st, pg), ct);

            return await response.HandleResponse<Page<StudiosResponse>>(ct);
        }

        /******/

        /* Post Requests For Studio */

        public async Task<StudioResponse?> Create(CreateStudioRequest studio, CancellationToken ct)
        {
            var response = await _httpClient.PostAsJsonAsync<CreateStudioRequest>(studiosUri, studio, ct);

            return await response.HandleResponse<StudioResponse>(ct);
        }
        public async Task<FilmographyResponse?> CreateFilmographyFor(string studioId, CreateFilmographyRequest filmography, CancellationToken ct)
        {
            var response = await _httpClient.PostAsJsonAsync<CreateFilmographyRequest>(studiosUri + $"/{studioId}/filmography", filmography, ct);

            return await response.HandleResponse<FilmographyResponse>(ct);
        }

        /******/

        /* Put Requests For Studio */

        public async Task<StudioResponse?> Update(string id, UpdateStudioRequest studio, CancellationToken ct)
        {
            var response = await _httpClient.PutAsJsonAsync<UpdateStudioRequest>(studiosUri + $"/{id}", studio, ct);

            return await response.HandleResponse<StudioResponse>(ct);
        }
        public async Task<StudioResponse?> UpdateMain(string id, UpdateStudioRequest studio, CancellationToken ct)
        {
            var response = await _httpClient.PutAsJsonAsync<UpdateStudioRequest>(studiosUri + $"/{id}/main", studio, ct);

            return await response.HandleResponse<StudioResponse>(ct);
        }
        public async Task<UpdatePictureResponse?> UpdatePhoto(string studioId, UpdatePictureRequest picture, CancellationToken ct)
        {
            var response = await _httpClient.PutAsJsonAsync<UpdatePictureRequest>(studiosUri + $"/{studioId}/picture", picture, ct);

            return await response.HandleResponse<UpdatePictureResponse>(ct);
        }
        public async Task<FilmographyResponse?> UpdateFilmography(string? studioId, string filmographyId, UpdateFilmographyRequest filmography, CancellationToken ct)
        {
            var response = await _httpClient.PutAsJsonAsync<UpdateFilmographyRequest>(studiosUri + (studioId == null ? "" : $"/{studioId}") + $"/filmography/{filmographyId}", filmography,  ct);

            return await response.HandleResponse<FilmographyResponse>(ct);
        }

        /******/

        /* Delete Requests For Studio */

        public async Task<bool> Delete(string id, CancellationToken ct)
        {
            var response = await _httpClient.DeleteAsync(studiosUri + $"/{id}", ct);

            return response.IsSuccessStatusCode;
        }
        public async Task<bool> DeleteFilmography(string? studioId, string filmographyId, CancellationToken ct)
        {
            var response = await _httpClient.DeleteAsync(studiosUri + (studioId == null ? "" : $"/{studioId}") + $"/filmography/{filmographyId}", ct);

            return response.IsSuccessStatusCode;
        }

        /******/

    }
}
