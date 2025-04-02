using MediatR;
using CinemaDataService.Infrastructure.Models.CinemaDTO;


namespace CinemaDataService.Api.Queries.CinemaQueries
{
    public class CinemaQuery : IRequest<CinemaResponse>
    {
        public CinemaQuery(string id)
        {
            Id = id;
        }

        public string Id { get; }
    }
}
