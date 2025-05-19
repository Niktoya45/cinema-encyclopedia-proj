using MediatR;
using UserDataService.Infrastructure.Models.RatingDTO;

namespace UserDataService.Api.Queries.UserQueries
{
    public class UserCinemaRatingQuery:IRequest<RatingResponse?>
    {
        public UserCinemaRatingQuery(string userId, string cinemaId)
        {
            UserId = userId;
            CinemaId = cinemaId;
        }

        public string UserId { get; set; }
        public string CinemaId { get; set; }
    }
}
