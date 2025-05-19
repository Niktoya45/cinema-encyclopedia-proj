using MediatR;
using UserDataService.Infrastructure.Models.LabeledDTO;
using UserDataService.Domain.Aggregates.UserAggregate;

namespace UserDataService.Api.Queries.UserQueries
{
    public class UserLabeledQuery : IRequest<IEnumerable<LabeledResponse>>
    {
        public UserLabeledQuery(string id, Label? label = null, string? userId = null)
        {
            Id = id;
            Label = label;
            UserId = userId;
        }

        public string Id { get; }
        public Label? Label { get; }
        public string? UserId { get; }
    }
}
