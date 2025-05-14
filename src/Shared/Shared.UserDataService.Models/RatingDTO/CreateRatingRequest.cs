
namespace Shared.UserDataService.Models.RatingDTO
{
    public class CreateRatingRequest
    {
        public string CinemaId { get; set; }
        public double Rating { get; set; }
    }
}
