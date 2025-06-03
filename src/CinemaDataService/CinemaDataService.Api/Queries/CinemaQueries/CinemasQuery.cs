using MediatR;
using CinemaDataService.Infrastructure.Models.CinemaDTO;
using CinemaDataService.Infrastructure.Models.SharedDTO;

namespace CinemaDataService.Api.Queries.CinemaQueries
{
    public class CinemasQuery
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
