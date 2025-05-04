using MediatR;
using CinemaDataService.Infrastructure.Models.StudioDTO;
using CinemaDataService.Infrastructure.Repositories.Utils;

namespace CinemaDataService.Api.Queries.StudioQueries
{
    public class StudiosQuery : IRequest<IEnumerable<StudiosResponse>>
    {
        public StudiosQuery(SortBy? sort=null, Pagination? pagination=null)
        {
            Sort = sort;
            Pg = pagination ?? new Pagination(0 , Pagination._max);
        }
        public SortBy? Sort { get; }
        public Pagination Pg { get; }
    }
}
