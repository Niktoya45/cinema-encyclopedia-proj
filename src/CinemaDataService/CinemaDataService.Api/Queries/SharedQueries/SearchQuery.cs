using CinemaDataService.Infrastructure.Models.SharedDTO;
using MediatR;

namespace CinemaDataService.Api.Queries.SharedQueries
{
    public class SearchQuery : IRequest<IEnumerable<SearchResponse>>
    {
        public SearchQuery(
            string search,
            Pagination? pagination = null
            )
        {
            Search = search.ToLower().Split();
            Pg = pagination ?? new Pagination(0, Pagination._max);
        }
        public string[] Search { get; }
        public Pagination Pg { get; }
    }
}
