using MediatR;
using UserDataService.Infrastructure.Models.UserDTO;


namespace UserDataService.Api.Queries.UserQueries
{
    public class UserQuery : IRequest<GetUserResponse>
    {
        public UserQuery(string id, string userId)
        {
            Id = id;
            UserId = userId;
        }

        public string Id { get; }
        public string UserId { get; }
    }
}
