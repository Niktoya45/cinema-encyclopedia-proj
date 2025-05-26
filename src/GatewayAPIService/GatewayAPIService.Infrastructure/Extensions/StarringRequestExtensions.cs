using Shared.CinemaDataService.Models.RecordDTO;

namespace GatewayAPIService.Infrastructure.Extensions
{
    public static class StarringRequestExtensions
    {
        public static bool SameCommons(this UpdateStarringRequest request, StarringResponse? response)
        {
            return   response == null ||
                     request.Name == response.Name
                  && request.Jobs == response.Jobs
                  && request.Picture == response.Picture;

        }

    }
}
