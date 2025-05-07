using CinemaDataService.Infrastructure.Models.PersonDTO;
using CinemaDataService.Infrastructure.Repositories.Utils;
using MediatR;

namespace CinemaDataService.Api.Queries.PersonQueries
{
    public class PersonsSearchQuery : IRequest<IEnumerable<PersonsResponse>>
    {
        public SortBy? Sort { get; }
        public Pagination Pg { get; }
        public string[] Search { get; }

        public PersonsSearchQuery(string search, SortBy? sort = null, Pagination? pagination = null, string? email = null)
        {
            Search = search.ToLower().Split();
            Sort = sort;
            Pg = pagination ?? new Pagination(0, Pagination._max);
        }
    }
}
