using CinemaDataService.Infrastructure.Pagination;
using CinemaDataService.Infrastructure.Sort;

namespace CinemaDataService.Api.Queries.StudioQueries
{
    public class StudiosYearQuery:StudiosQuery
    {
        public StudiosYearQuery(int year, SortBy? sort = null, Pagination? pagination = null):base(sort, pagination)
        {
            Year = year;
        }
        public int Year { get; }
    }
}
