using MediatR;
using UserDataService.Infrastructure.Models.UserDTO;


namespace UserDataService.Api.Queries.UserQueries
{
    public class UserQuery : IRequest<GetUserResponse>
    {
        public UserQuery(string id)
        {
            Id = id;
        }

        public string Id { get; }
    }
}
