using CinemaDataService.Infrastructure.Models.SharedDTO;

namespace CinemaDataService.Api.Queries.CinemaQueries
{
    public class CinemasYearSpansQuery:CinemasQuery
    {
        public CinemasYearSpansQuery(int[] yearsLower, int yearSpan, SortBy? sort = null, Pagination? pagination = null) : base(sort, pagination)
        {
            YearsLower = yearsLower;
            YearSpan = yearSpan;
        }
        public int[] YearsLower { get; }
        public int YearSpan { get; }
    }
}
