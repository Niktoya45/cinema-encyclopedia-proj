
using CinemaDataService.Domain.Aggregates.CinemaAggregate;

namespace CinemaDataService.Infrastructure.Models.SharedDTO
{
    public class UpdateRatingResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public RatingScore Rating { get; set; }
    }
}
