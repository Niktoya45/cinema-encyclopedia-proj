using MediatR;
using UserDataService.Infrastructure.Models.RatingDTO;

namespace UserDataService.Api.Commands.UserCommands.DeleteCommands
{
    public class DeleteRatingCommand : IRequest<RatingResponse>
    {
        public DeleteRatingCommand(
            string cinemaId,
            string userId
            )
        {
            CinemaId = cinemaId;
            UserId = userId;
        }
        public string CinemaId { get; set; }
        public string UserId { get; set; }
    }
}
