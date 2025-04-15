using AutoMapper;
using MediatR;
using CinemaDataService.Infrastructure.Models.StudioDTO;
using CinemaDataService.Domain.Aggregates.StudioAggregate;
//using CinemaDataService.Api.Exceptions.StudioExceptions;
using CinemaDataService.Infrastructure.Repositories.Abstractions;

namespace CinemaDataService.Api.Queries.StudioQueries
{

    public class StudioQueryHandler : IRequestHandler<StudioQuery, StudioResponse>
    {
        IStudioRepository _repository;
        IMapper _mapper;
        public StudioQueryHandler(IStudioRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<StudioResponse> Handle(StudioQuery request, CancellationToken cancellationToken)
        {
            Studio? Studio = await _repository.FindById(request.Id, cancellationToken);

            //if (Studio == null)
                // handle


            return _mapper.Map<StudioResponse>(Studio);
        }
    }
}
