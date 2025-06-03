using CinemaDataService.Infrastructure.Models.CinemaDTO;
using CinemaDataService.Infrastructure.Models.SharedDTO;
using MediatR;

namespace CinemaDataService.Api.Queries.CinemaQueries
{
    public class CinemasQueryCommonWrapper : IRequest<Page<CinemasResponse>>
    {
        public CinemasQueryCommonWrapper(CinemasQuery query)
        {
            Query = query;
        }

        public CinemasQuery Query { get; }
    }
}
