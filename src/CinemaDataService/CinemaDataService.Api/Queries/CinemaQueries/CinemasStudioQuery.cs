using CinemaDataService.Infrastructure.Repositories.Utils;

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
