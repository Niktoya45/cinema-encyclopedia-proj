using MediatR;
using UserDataService.Infrastructure.Models.UserDTO;


namespace UserDataService.Api.Queries.UserQueries
{
    public class UserQuery : IRequest<UserResponse>
    {
        public UserQuery(string id, string? userId= null)
        {
            Id = id;
            UserId = userId;
        }

        public string Id { get; }
        public string? UserId { get; }
    }
}
