using Shared.CinemaDataService.Models.RecordDTO;

namespace Shared.CinemaDataService.Models.SharedDTO
{
    public class UpdateRatingResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public RatingScore Rating { get; set; }
    }
}
