using CinemaDataService.Infrastructure.Models.SharedDTO;

namespace CinemaDataService.Api.Queries.CinemaQueries
{
    public class CinemasSearchPageQuery:CinemasQuery
    {
        public CinemasSearchPageQuery(string search, SortBy st, Pagination pg):base(st, pg) 
        {
            Search = search;
        }

        public string Search { get; set; }
    }
}
