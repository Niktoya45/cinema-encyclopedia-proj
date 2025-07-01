using UserDataService.Domain.Aggregates.UserAggregate;

namespace UserDataService.Infrastructure.Models.LabeledDTO
{
    public class UpdateRatingRequest
    {
        public string CinemaId { get; set; }
        public double Rating { get; set; }
    }
}
