using MediatR;
using UserDataService.Infrastructure.Models.LabeledDTO;

namespace UserDataService.Api.Queries.UserQueries
{
    public class UserLabeledQuery : IRequest<IEnumerable<LabeledResponse>>
    {
        public UserLabeledQuery(string id, string? userId = null)
        {
            Id = id;
            UserId = userId;
        }

        public string Id { get; }
        public string? UserId { get; }
    }
}
