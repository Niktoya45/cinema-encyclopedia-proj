using CinemaDataService.Infrastructure.Models.CinemaDTO;
using CinemaDataService.Infrastructure.Repositories.Utils;
using MediatR;

namespace CinemaDataService.Api.Queries.CinemaQueries
{
    public class CinemasSearchQuery : IRequest<IEnumerable<CinemasResponse>>
    {
        public CinemasSearchQuery(
            string search,
            SortBy? sort = null,
            Pagination? pagination = null
            )
        {
            Search = search.ToLower().Split();
            Sort = sort;
            Pg = pagination ?? new Pagination(0, Pagination._max);
        }
        public string[] Search { get;}
        public SortBy? Sort { get; }
        public Pagination Pg { get; }
    }
}
