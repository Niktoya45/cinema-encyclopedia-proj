

using GatewayAPIService.Infrastructure.Extensions;
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
        const string cinemasUri = "/api/cinemas";

        HttpClient _httpClient;
   
        string sort_paginate(SortBy? st, Pagination? pg) =>  ( st == null ? "" : $"order={st.Order}&field={st.Field}&" ) 
                                                          + ( pg == null ? "" : $"skip={pg.Skip}&take={pg.Take}"      );

        public CinemaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /* Get Requests For Cinema */
        public async Task<Page<CinemasResponse>?> Get(CancellationToken ct, SortBy? st = null, Pagination? pg = null)
        {
            var response = await _httpClient.GetAsync(cinemasUri + "?" + sort_paginate(st, pg), ct);

            return await response.HandleResponse<Page<CinemasResponse>>(ct);
        }
        public async Task<CinemaResponse?> GetById(string id, CancellationToken ct)
        {
            var response = await _httpClient.GetAsync(cinemasUri+$"/{id}", ct);

            return await response.HandleResponse<CinemaResponse>(ct);
        }
        public async Task<StarringResponse?> GetStarringById(string cinemaId, string starringId, CancellationToken ct) 
        {
            var response = await _httpClient.GetAsync(cinemasUri+$"{cinemaId}/starrings/{starringId}");

            return await response.HandleResponse<StarringResponse>(ct);
        }
        public async Task<ProductionStudioResponse?> GetProductionStudioById(string cinemaId, string studioId, CancellationToken ct)
        {
            var response = await _httpClient.GetAsync(cinemasUri + $"{cinemaId}/production-studios/{studioId}", ct);

            return await response.HandleResponse<ProductionStudioResponse>(ct);
        }
        public async Task<Page<CinemasResponse>?> GetByIds(string[] ids, CancellationToken ct, Pagination? pg = null) 
        {
            var response = await _httpClient.PostAsJsonAsync(cinemasUri + "/indexes?" + sort_paginate(null, pg), ids, ct);

            return await response.HandleResponse<Page<CinemasResponse>>(ct);
        }
        public async Task<IEnumerable<SearchResponse>?> GetBySearch(string search, CancellationToken ct, Pagination? pg = null) 
        {
            var response = await _httpClient.GetAsync(cinemasUri + "/search" + $"/{search}?" + sort_paginate(null, pg), ct);

            return await response.HandleResponse<IEnumerable<SearchResponse>>(ct);
        }
        public async Task<Page<CinemasResponse>?> GetByYear(int year, CancellationToken ct, SortBy? st = null, Pagination? pg = null)
        {
            var response = await _httpClient.GetAsync(cinemasUri + "/year" + $"/{year}?" + sort_paginate(st, pg), ct);

            return await response.HandleResponse<Page<CinemasResponse>>(ct);
        }
        public async Task<Page<CinemasResponse>?> GetByGenre(Genre genre, CancellationToken ct, SortBy? st = null, Pagination? pg = null)
        {
            var response = await _httpClient.GetAsync(cinemasUri + "/genres" + $"/{genre}?" + sort_paginate(st, pg), ct);

            return await response.HandleResponse<Page<CinemasResponse>>(ct);
        }
        public async Task<Page<CinemasResponse>?> GetByLanguage(Language language, CancellationToken ct, SortBy? st = null, Pagination? pg = null)
        {
            var response = await _httpClient.GetAsync(cinemasUri + "/language" + $"/{language}?" + sort_paginate(st, pg), ct);

            return await response.HandleResponse<Page<CinemasResponse>>(ct);
        }
        public async Task<Page<CinemasResponse>?> GetByStudioId(string studioId, CancellationToken ct, SortBy? st = null, Pagination? pg = null)
        {
            var response = await _httpClient.GetAsync(cinemasUri + "/studio" + $"/{studioId}?" + sort_paginate(st, pg), ct);

            return await response.HandleResponse<Page<CinemasResponse>>(ct);
        }

        /******/

        /* Post Requests For Cinema */

        public async Task<CinemaResponse?> Create(CreateCinemaRequest cinema, CancellationToken ct) 
        {
            var response = await _httpClient.PostAsJsonAsync(cinemasUri, cinema, ct);

            return await response.HandleResponse<CinemaResponse>(ct);
        }
        public async Task<ProductionStudioResponse?> CreateProductionStudioFor(string cinemaId, CreateProductionStudioRequest studio, CancellationToken ct)
        {
            var response = await _httpClient.PostAsJsonAsync(cinemasUri + $"/{cinemaId}/production-studios", studio, ct);

            return await response.HandleResponse<ProductionStudioResponse>(ct);
        }
        public async Task<StarringResponse?> CreateStarringFor(string cinemaId, CreateStarringRequest starring, CancellationToken ct)
        {
            var response = await _httpClient.PostAsJsonAsync(cinemasUri + $"/{cinemaId}/starrings", starring, ct);

            return await response.HandleResponse<StarringResponse>(ct);
        }

        /******/

        /* Put Requests For Cinema */

        public async Task<CinemaResponse?> Update(string id, UpdateCinemaRequest cinema, CancellationToken ct)
        {
            var response = await _httpClient.PutAsJsonAsync(cinemasUri + $"/{id}", cinema, ct);

            return await response.HandleResponse<CinemaResponse>(ct);
        }

        public async Task<UpdatePictureResponse?> UpdatePhoto(string cinemaId, UpdatePictureRequest picture, CancellationToken ct)
        {
            var response = await _httpClient.PutAsJsonAsync(cinemasUri + $"/{cinemaId}/picture", picture, ct);

            return await response.HandleResponse<UpdatePictureResponse>(ct);
        }
        public async Task<UpdateRatingResponse?> UpdateRating(string cinemaId, UpdateRatingRequest rating, CancellationToken ct)
        {
            var response = await _httpClient.PutAsJsonAsync(cinemasUri + $"/{cinemaId}/rating", rating, ct);

            return await response.HandleResponse<UpdateRatingResponse>(ct);
        }
        public async Task<ProductionStudioResponse?> UpdateProductionStudio(string? cinemaId, string studioId, UpdateProductionStudioRequest studio, CancellationToken ct)
        {
            var response = await _httpClient.PutAsJsonAsync(cinemasUri + (cinemaId == null ? "" : $"/{cinemaId}") + $"/production-studios/{studioId}", studio, ct);

            return await response.HandleResponse<ProductionStudioResponse>(ct);
        }
        public async Task<StarringResponse?> UpdateStarring(string? cinemaId, string starringId, UpdateStarringRequest starring, CancellationToken ct)
        {
            var response = await _httpClient.PutAsJsonAsync(cinemasUri + (cinemaId == null? "" : $"/{cinemaId}") + $"/starrings/{starringId}", starring, ct);

            return await response.HandleResponse<StarringResponse>(ct);
        }

        /******/

        /* Delete Requests For Cinema */

        public async Task<bool> Delete(string id, CancellationToken ct)
        {
            var response = await _httpClient.DeleteAsync(cinemasUri + $"/{id}", ct);

            return response.IsSuccessStatusCode;
        }
        public async Task<bool> DeleteProductionStudio(string? cinemaId, string studioId, CancellationToken ct)
        {
            var response = await _httpClient.DeleteAsync(cinemasUri + (cinemaId == null ? "" : $"/{cinemaId}") + $"/{studioId}", ct);

            return response.IsSuccessStatusCode;
        }
        public async Task<bool> DeleteStarring(string? cinemaId, string starringId, CancellationToken ct)
        {
            var response = await _httpClient.DeleteAsync(cinemasUri + (cinemaId == null ? "" : $"/{cinemaId}") + $"/{starringId}", ct);

            return response.IsSuccessStatusCode;
        }

        /******/
    }
}
