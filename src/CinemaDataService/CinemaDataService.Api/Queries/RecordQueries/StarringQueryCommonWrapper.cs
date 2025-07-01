using CinemaDataService.Infrastructure.Models.RecordDTO;

namespace CinemaDataService.Api.Queries.RecordQueries
{
    public class StarringQueryCommonWrapper:RecordQueryCommonWrapper<StarringQuery, StarringResponse>
    {
        public StarringQueryCommonWrapper(StarringQuery query) : base(query)
        {
        }
    }
}
