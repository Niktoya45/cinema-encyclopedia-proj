using CinemaDataService.Infrastructure.Models.StudioDTO;
using CinemaDataService.Infrastructure.Repositories.Utils;
using MediatR;

namespace CinemaDataService.Api.Queries.StudioQueries
{
    public class StudiosSearchQuery : IRequest<IEnumerable<StudiosResponse>>
    {
        public StudiosSearchQuery(string search, SortBy? sort = null, Pagination? pagination = null)
        {
            Search = search.ToLower().Split();
            Sort = sort;
            Pg = pagination ?? new Pagination(0, Pagination._max);
        }
        public SortBy? Sort { get; }
        public Pagination Pg { get; }
        public string[] Search { get; }
    }
}
