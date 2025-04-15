using CinemaDataService.Infrastructure.Pagination;
using CinemaDataService.Infrastructure.Sort;

namespace CinemaDataService.Api.Queries.CinemaQueries
{
    public class CinemasStudioQuery:CinemasQuery
    {
        public CinemasStudioQuery(string studioId, SortBy? sort = null, Pagination? pagination = null):base(sort, pagination)
        {
            StudioId = studioId;
        }
        public string StudioId { get; }
    }
}
