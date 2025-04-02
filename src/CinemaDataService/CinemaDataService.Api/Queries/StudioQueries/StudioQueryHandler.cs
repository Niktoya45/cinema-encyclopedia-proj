using AutoMapper;
using MediatR;
using CinemaDataService.Infrastructure.Models.StudioDTO;
using CinemaDataService.Domain.Aggregates.StudioAggregate;
using CinemaDataService.Api.Exceptions.StudioExceptions;
using CinemaDataService.Infrastructure.Repositories.UnitOfWork;

namespace CinemaDataService.Api.Queries.StudioQueries
{

    public class StudioQueryHandler : IRequestHandler<StudioQuery, StudioResponse>
    {
        IUnitOfWork _unitOfWork;
        IMapper _mapper;
        public StudioQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<StudioResponse> Handle(StudioQuery request, CancellationToken cancellationToken)
        {
            Studio? Studio = await _unitOfWork.Studios.GetById(request.Id, cancellationToken);

            if (Studio == null)
                throw new TrialStudioNotFoundException(request.Id);


            return _mapper.Map<StudioResponse>(Studio);
        }
    }
}
