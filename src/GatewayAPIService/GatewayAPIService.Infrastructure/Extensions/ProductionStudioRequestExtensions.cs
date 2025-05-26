
using Shared.CinemaDataService.Models.RecordDTO;

namespace GatewayAPIService.Infrastructure.Extensions
{
    public static class ProductionStudioRequestExtensions
    {
        public static bool SameCommons(this UpdateProductionStudioRequest request, ProductionStudioResponse? response)
        {
            return   response == null ||
                     request.Name == response.Name
                  && request.Picture == response.Picture;

        }
    }
}
