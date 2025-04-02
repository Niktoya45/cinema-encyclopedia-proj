using MediatR;
using CinemaDataService.Infrastructure.Pagination;
using CinemaDataService.Infrastructure.Models.StudioDTO;
using CinemaDataService.Infrastructure.Sort;

namespace CinemaDataService.Api.Queries.StudioQueries
{
    public class StudiosQuery : IRequest<IEnumerable<StudiosResponse>>
    {
        public StudiosQuery(SortBy? sort=null, Pagination? pagination=null)
        {
            Sort = sort;
            Pg = pagination ?? new Pagination();
        }
        public SortBy? Sort { get; }
        public Pagination Pg { get; }
    }
}
