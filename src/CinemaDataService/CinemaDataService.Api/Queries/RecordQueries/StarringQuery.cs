using CinemaDataService.Infrastructure.Models.RecordDTO;
using MediatR;

namespace CinemaDataService.Api.Queries.RecordQueries
{
    public class StarringQuery : IRequest<StarringResponse>
    {
        public StarringQuery(string parentId, string id)
        {
            ParentId = parentId;
            Id = id;
        }

        public string ParentId { get; set; }
        public string Id { get; set; }
    }
}
