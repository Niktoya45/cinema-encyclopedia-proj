using CinemaDataService.Domain.Aggregates.CinemaAggregate;
using CinemaDataService.Infrastructure.Models.SharedDTO;

namespace CinemaDataService.Api.Queries.CinemaQueries
{
    public class CinemasGenresQuery:CinemasQuery
    {
        public CinemasGenresQuery(Genre genres, SortBy? sort = null, Pagination? pagination = null):base(sort, pagination)
        {
            Genres = genres;
        }
        public Genre Genres { get; }
    }
}
