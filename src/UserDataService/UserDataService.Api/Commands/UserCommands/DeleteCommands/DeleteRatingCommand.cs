using MediatR;
using UserDataService.Infrastructure.Models.RatingDTO;

namespace UserDataService.Api.Commands.UserCommands.DeleteCommands
{
    public class DeleteRatingCommand : IRequest<IEnumerable<RatingResponse>>
    {
        public DeleteRatingCommand(
            string userId,
            string? cinemaId
            )
        {
            CinemaId = cinemaId;
            UserId = userId;
        }
        public string? CinemaId { get; set; }
        public string UserId { get; set; }
    }
}
