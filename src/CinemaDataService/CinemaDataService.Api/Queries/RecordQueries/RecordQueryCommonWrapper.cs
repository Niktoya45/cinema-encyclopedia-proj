using CinemaDataService.Api.Queries.CinemaQueries;
using MediatR;

namespace CinemaDataService.Api.Queries.RecordQueries
{
    public class RecordQueryCommonWrapper<TQuery, TResponse>: IRequest<TResponse>
    {
        public RecordQueryCommonWrapper(TQuery query)
        {
            Query = query;
        }

        public TQuery Query { get; }
    }
}
