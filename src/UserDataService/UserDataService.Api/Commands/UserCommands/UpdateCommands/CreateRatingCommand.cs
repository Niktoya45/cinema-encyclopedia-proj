using MediatR;
using UserDataService.Infrastructure.Models.RatingDTO;

namespace UserDataService.Api.Commands.UserCommands.UpdateCommands
{
    public class UpdateRatingCommand : IRequest<RatingResponse>
    {
        public UpdateRatingCommand(
            string userId,
            string cinemaId,
            double rating
            )
        {
            CinemaId = cinemaId;
            UserId = userId;
            Rating = rating;
        }
        public string CinemaId { get; set; }
        public string UserId { get; set; }
        public double Rating { get; set; }
    }
}
