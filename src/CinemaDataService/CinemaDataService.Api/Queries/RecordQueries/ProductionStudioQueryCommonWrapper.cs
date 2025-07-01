using CinemaDataService.Infrastructure.Models.RecordDTO;

namespace CinemaDataService.Api.Queries.RecordQueries
{
    public class ProductionStudioQueryCommonWrapper:RecordQueryCommonWrapper<ProductionStudioQuery, ProductionStudioResponse>
    {
        public ProductionStudioQueryCommonWrapper(ProductionStudioQuery query) : base(query)
        {
        }
    }
}
