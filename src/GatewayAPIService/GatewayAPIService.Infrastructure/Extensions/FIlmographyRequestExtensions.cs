using Shared.CinemaDataService.Models.RecordDTO;

namespace GatewayAPIService.Infrastructure.Extensions
{
    public static class FilmographyRequestExtensions
    {
        public static bool SameCommons(this UpdateFilmographyRequest request, FilmographyResponse? response)
        {
            return   response == null ||
                     request.Name == response.Name
                  && request.Year == response.Year
                  && request.Picture == response.Picture;

        }
    }
}
