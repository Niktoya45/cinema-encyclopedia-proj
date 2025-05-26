

using GatewayAPIService.Infrastructure.Services.CinemaService;
using Shared.CinemaDataService.Models.CinemaDTO;
using Shared.CinemaDataService.Models.Flags;
using Shared.CinemaDataService.Models.RecordDTO;
using Shared.CinemaDataService.Models.SharedDTO;
using System.Net.Http.Json;

namespace GatewayAPIService.Infrastructure.Services.CinemaService
{
    public class CinemaService:ICinemaService
    {
        const string Url = "/api/cinemas";

        HttpClient _httpClient;
   
        string SortPaginate(SortBy? st, Pagination? pg) =>  ( st == null ? "" : $"order={st.Order}&field={st.Field}&" ) 
                                                          + ( pg == null ? "" : $"skip={pg.Skip}&take={pg.Take}"      );

        public CinemaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /* Get Requests For Cinema */
        public async Task<Page<CinemasResponse>?> Get(CancellationToken ct, SortBy? st = null, Pagination? pg = null)
        {
            return await _httpClient.GetFromJsonAsync<Page<CinemasResponse>?>(Url + "?" + SortPaginate(st, pg), ct);
        }
        public async Task<CinemaResponse?> GetById(string id, CancellationToken ct)
        {
            return await _httpClient.GetFromJsonAsync<CinemaResponse>(Url+$"/{id}", ct);
        }
        public async Task<StarringResponse?> GetStarringById(string cinemaId, string starringId, CancellationToken ct) 
        {
            return await _httpClient.GetFromJsonAsync<StarringResponse>(Url+$"{cinemaId}/starrings/{starringId}");
        }
        public async Task<ProductionStudioResponse?> GetProductionStudioById(string cinemaId, string studioId, CancellationToken ct)
        {
            return await _httpClient.GetFromJsonAsync<ProductionStudioResponse>(Url + $"{cinemaId}/production-studios/{studioId}");
        }
        public async Task<Page<CinemasResponse>?> GetByIds(string[] ids, CancellationToken ct, Pagination? pg = null) 
        {
            var response = await _httpClient.PostAsJsonAsync(Url + "/indexes?" + SortPaginate(null, pg), ids, ct);

            return await response.Content.ReadFromJsonAsync<Page<CinemasResponse>?>();
        }
        public async Task<IEnumerable<SearchResponse>?> GetBySearch(string search, CancellationToken ct, Pagination? pg = null) 
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<SearchResponse>>(Url + "/search" + $"/{search}?" + SortPaginate(null, pg), ct);
        }
        public async Task<Page<CinemasResponse>?> GetByYear(int year, CancellationToken ct, SortBy? st = null, Pagination? pg = null)
        {
            return await _httpClient.GetFromJsonAsync<Page<CinemasResponse>>(Url + "/year" + $"/{year}?" + SortPaginate(st, pg), ct);
        }
        public async Task<Page<CinemasResponse>?> GetByGenre(Genre genre, CancellationToken ct, SortBy? st = null, Pagination? pg = null)
        {
            return await _httpClient.GetFromJsonAsync<Page<CinemasResponse>>(Url + "/genres" + $"/{genre}?" + SortPaginate(st, pg), ct);
        }
        public async Task<Page<CinemasResponse>?> GetByLanguage(Language language, CancellationToken ct, SortBy? st = null, Pagination? pg = null)
        {
            return await _httpClient.GetFromJsonAsync<Page<CinemasResponse>>(Url + "/language" + $"/{language}?" + SortPaginate(st, pg), ct);
        }
        public async Task<Page<CinemasResponse>?> GetByStudioId(string studioId, CancellationToken ct, SortBy? st = null, Pagination? pg = null)
        {
            return await _httpClient.GetFromJsonAsync<Page<CinemasResponse>>(Url + "/studio" + $"/{studioId}?" + SortPaginate(st, pg), ct);
        }

        /******/

        /* Post Requests For Cinema */

        public async Task<CinemaResponse?> Create(CreateCinemaRequest cinema, CancellationToken ct) 
        {
            var response = await _httpClient.PostAsJsonAsync(Url, cinema, ct);

            return await response.Content.ReadFromJsonAsync<CinemaResponse?>();
        }
        public async Task<ProductionStudioResponse?> CreateProductionStudioFor(string cinemaId, CreateProductionStudioRequest studio, CancellationToken ct)
        {
            var response = await _httpClient.PostAsJsonAsync(Url + $"/{cinemaId}/production-studios", studio, ct);

            return await response.Content.ReadFromJsonAsync<ProductionStudioResponse?>();
        }
        public async Task<StarringResponse?> CreateStarringFor(string cinemaId, CreateStarringRequest starring, CancellationToken ct)
        {
            var response = await _httpClient.PostAsJsonAsync(Url + $"/{cinemaId}/starrings", starring, ct);

            return await response.Content.ReadFromJsonAsync<StarringResponse?>();
        }

        /******/

        /* Put Requests For Cinema */

        public async Task<CinemaResponse?> Update(string id, UpdateCinemaRequest cinema, CancellationToken ct)
        {
            var response = await _httpClient.PutAsJsonAsync(Url + $"/{id}", cinema, ct);

            return await response.Content.ReadFromJsonAsync<CinemaResponse?>();
        }

        public async Task<UpdatePictureResponse?> UpdatePhoto(string cinemaId, UpdatePictureRequest picture, CancellationToken ct)
        {
            var response = await _httpClient.PutAsJsonAsync(Url + $"/{cinemaId}/picture", picture, ct);

            return await response.Content.ReadFromJsonAsync<UpdatePictureResponse?>();
        }
        public async Task<UpdateRatingResponse?> UpdateRating(string cinemaId, UpdateRatingRequest rating, CancellationToken ct)
        {
            var response = await _httpClient.PutAsJsonAsync(Url + $"/{cinemaId}/rating", rating, ct);

            return await response.Content.ReadFromJsonAsync<UpdateRatingResponse?>();
        }
        public async Task<ProductionStudioResponse?> UpdateProductionStudio(string? cinemaId, string studioId, UpdateProductionStudioRequest studio, CancellationToken ct)
        {
            var response = await _httpClient.PutAsJsonAsync(Url + (cinemaId == null ? "" : $"/{cinemaId}") + $"/production-studios/{studioId}", studio, ct);

            return await response.Content.ReadFromJsonAsync<ProductionStudioResponse?>();
        }
        public async Task<StarringResponse?> UpdateStarring(string? cinemaId, string starringId, UpdateStarringRequest starring, CancellationToken ct)
        {
            var response = await _httpClient.PutAsJsonAsync(Url + (cinemaId == null? "" : $"/{cinemaId}") + $"/starrings/{starringId}", starring, ct);

            return await response.Content.ReadFromJsonAsync<StarringResponse?>();
        }

        /******/

        /* Delete Requests For Cinema */

        public async Task<bool> Delete(string id, CancellationToken ct)
        {
            var response = await _httpClient.DeleteAsync(Url + $"/{id}", ct);

            return response.IsSuccessStatusCode;
        }
        public async Task<bool> DeleteProductionStudio(string? cinemaId, string studioId, CancellationToken ct)
        {
            var response = await _httpClient.DeleteAsync(Url + (cinemaId == null ? "" : $"/{cinemaId}") + $"/{studioId}", ct);

            return response.IsSuccessStatusCode;
        }
        public async Task<bool> DeleteStarring(string? cinemaId, string starringId, CancellationToken ct)
        {
            var response = await _httpClient.DeleteAsync(Url + (cinemaId == null ? "" : $"/{cinemaId}") + $"/{starringId}", ct);

            return response.IsSuccessStatusCode;
        }

        /******/
    }
}
