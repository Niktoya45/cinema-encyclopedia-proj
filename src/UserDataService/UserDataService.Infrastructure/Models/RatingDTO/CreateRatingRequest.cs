using UserDataService.Domain.Aggregates.UserAggregate;

namespace UserDataService.Infrastructure.Models.LabeledDTO
{
    public class CreateRatingRequest
    {
        public string CinemaId { get; set; }
        public double Rating { get; set; }
    }
}
