using CinemaDataService.Infrastructure.Repositories.Utils;

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
