using CinemaDataService.Api.Queries.PersonQueries;
using CinemaDataService.Infrastructure.Models.SharedDTO;

namespace CinemaDataService.Api.Queries.StudioQueries
{
    public class StudiosSearchPageQuery : StudiosQuery
    {
        public StudiosSearchPageQuery(string search, SortBy? st = null, Pagination? pg = null) : base(st, pg)
        {
            Search = search;
        }

        public string Search { get; }
    }
}
