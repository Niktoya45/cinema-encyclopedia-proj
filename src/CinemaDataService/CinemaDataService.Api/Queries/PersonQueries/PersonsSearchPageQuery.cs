using CinemaDataService.Infrastructure.Models.SharedDTO;

namespace CinemaDataService.Api.Queries.PersonQueries
{
    public class PersonsSearchPageQuery:PersonsQuery
    {
        public PersonsSearchPageQuery(string search, SortBy? st = null, Pagination? pg = null):base(st, pg)
        { 
            Search = search;
        }

        public string Search { get; }
    }
}
