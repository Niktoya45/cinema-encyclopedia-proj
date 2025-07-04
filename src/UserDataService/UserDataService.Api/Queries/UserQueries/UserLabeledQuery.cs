using MediatR;
using UserDataService.Infrastructure.Models.LabeledDTO;
using UserDataService.Domain.Aggregates.UserAggregate;

namespace UserDataService.Api.Queries.UserQueries
{
    public class UserLabeledQuery : IRequest<IEnumerable<LabeledResponse>>
    {
        public UserLabeledQuery(string id, Label label = Label.None, string? cinemaId = null, string? userId = null)
        {
            Id = id;
            Label = label;
            CinemaId = cinemaId;
            UserId = userId;
        }

        public string Id { get; }
        public Label Label { get; }
        public string? CinemaId { get; }
        public string? UserId { get; }
    }
}
