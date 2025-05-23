using AutoMapper;
using CinemaDataService.Api.Exceptions.InfrastructureExceptions;
using CinemaDataService.Domain.Aggregates.CinemaAggregate;
using CinemaDataService.Domain.Aggregates.Shared;
using CinemaDataService.Infrastructure.Models.RecordDTO;
using CinemaDataService.Infrastructure.Repositories.Abstractions;
using MediatR;

namespace CinemaDataService.Api.Queries.RecordQueries
{
    public class ProductionStudioQueryHandler : IRequestHandler<ProductionStudioQuery, ProductionStudioResponse>
    {
        ICinemaRepository _cinemaRepository;
        IMapper _mapper;
        public ProductionStudioQueryHandler(ICinemaRepository cinemaRepository, IMapper mapper)
        {
            _cinemaRepository = cinemaRepository;
            _mapper = mapper;
        }
        public async Task<ProductionStudioResponse> Handle(ProductionStudioQuery request, CancellationToken cancellationToken)
        {
            StudioRecord? studio = null;

            switch (request)
            {

                case CinemaProductionStudioQuery csq:
                    studio = await _cinemaRepository.FindProductionStudioById(csq.ParentId, csq.Id, cancellationToken);
                    break;

                default:
                    break;
            }

            if (studio == null)
            {
                throw new NotFoundRecordException(request.ParentId, request.Id, "Cinemas");
            }


            return _mapper.Map<ProductionStudioResponse>(studio);
        }
    }
}
