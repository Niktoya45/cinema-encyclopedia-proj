using UserDataService.Domain.Aggregates.UserAggregate;

namespace UserDataService.Infrastructure.Models.RatingDTO
{
    public class RatingResponse
    {
        public string CinemaId { get; set; }
        public string UserId { get; set; }
        public double Rating { get; set; }
    }
}
