using CinemaDataService.Infrastructure.Models.CinemaDTO;
using CinemaDataService.Infrastructure.Models.SharedDTO;
using MediatR;

namespace CinemaDataService.Api.Queries.CinemaQueries
{
    public class CinemasIdQuery:CinemasQuery
    {
        public CinemasIdQuery(
            string[] ids,
            SortBy? sort = null
            ):base(sort, null)
        {
            Ids = ids;
        }
        public string[] Ids { get; }
    }
}
