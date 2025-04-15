using MediatR;
using CinemaDataService.Infrastructure.Pagination;
using CinemaDataService.Infrastructure.Models.PersonDTO;
using CinemaDataService.Infrastructure.Sort;

namespace CinemaDataService.Api.Queries.PersonQueries
{
    public class PersonsQuery : IRequest<IEnumerable<PersonsResponse>>
    {
        public SortBy? Sort { get; }
        public Pagination Pg { get; }

        public PersonsQuery(SortBy? sort = null, Pagination? pagination = null, string? email=null)
        {
            Sort = sort;
            Pg = pagination ?? new Pagination(0, Pagination._max);
        }
    }
}
