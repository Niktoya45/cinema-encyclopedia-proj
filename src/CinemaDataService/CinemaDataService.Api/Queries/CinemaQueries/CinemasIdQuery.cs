using CinemaDataService.Infrastructure.Models.CinemaDTO;
using CinemaDataService.Infrastructure.Models.SharedDTO;
using MediatR;

namespace CinemaDataService.Api.Queries.CinemaQueries
{
    public class CinemasIdQuery:CinemasQuery
    {
        public CinemasIdQuery(
            string[] ids,
            Pagination? pagination = null
            ):base(null, pagination)
        {
            Ids = ids;
        }
        public string[] Ids { get; }
    }
}
