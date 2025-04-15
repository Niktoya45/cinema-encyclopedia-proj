using MediatR;
using CinemaDataService.Infrastructure.Pagination;
using CinemaDataService.Infrastructure.Models.CinemaDTO;
using CinemaDataService.Infrastructure.Sort;

namespace CinemaDataService.Api.Queries.CinemaQueries
{
    public class CinemasQuery : IRequest<IEnumerable<CinemasResponse>>
    {
        public CinemasQuery(
            SortBy? sort = null,
            Pagination? pagination=null
            )
        {
            Sort = sort;
            Pg = pagination ?? new Pagination(0, Pagination._max);
        }
        public SortBy? Sort { get; }
        public Pagination Pg { get; }
    }
}
