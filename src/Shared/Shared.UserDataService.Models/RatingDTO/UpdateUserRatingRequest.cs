
namespace Shared.UserDataService.Models.RatingDTO
{
    public class UpdateUserRatingRequest
    {
        public string CinemaId { get; set; }
        public double Rating { get; set; }
    }
}
