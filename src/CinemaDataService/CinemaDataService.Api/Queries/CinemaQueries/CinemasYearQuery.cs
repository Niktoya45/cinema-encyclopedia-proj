using CinemaDataService.Infrastructure.Repositories.Utils;

namespace CinemaDataService.Api.Queries.CinemaQueries
{
    public class CinemasYearQuery:CinemasQuery
    {
        public CinemasYearQuery(int year, SortBy? sort = null, Pagination? pagination = null):base(sort, pagination)
        {
            Year = year;
        }
        public int Year { get; }
    }
}
