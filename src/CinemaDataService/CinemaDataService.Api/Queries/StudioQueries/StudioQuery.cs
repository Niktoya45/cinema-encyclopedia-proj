using MediatR;
using CinemaDataService.Infrastructure.Models.StudioDTO;


namespace CinemaDataService.Api.Queries.StudioQueries
{
    public class StudioQuery : IRequest<StudioResponse>
    {
        public StudioQuery(string id)
        {
            Id = id;
        }

        public string Id { get; }
    }
}
