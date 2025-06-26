using CinemaDataService.Infrastructure.Models.CinemaDTO;
using CinemaDataService.Infrastructure.Models.SharedDTO;
using MediatR;

namespace CinemaDataService.Api.Queries.StudioQueries
{
    public class StudiosIdQuery:StudiosQuery
    {
        public StudiosIdQuery(
            string[] ids,
            Pagination? pagination = null
            ):base(null, pagination)
        {
            Ids = ids;
        }
        public string[] Ids { get; }
    }
}
