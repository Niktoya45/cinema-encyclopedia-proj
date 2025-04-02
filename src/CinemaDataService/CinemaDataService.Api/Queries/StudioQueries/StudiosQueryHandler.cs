using AutoMapper;
using MediatR;
using CinemaDataService.Api.Queries;
using CinemaDataService.Domain.Aggregates.StudioAggregate;
using CinemaDataService.Infrastructure.Models.StudioDTO;
using CinemaDataService.Api.Exceptions.StudioExceptions;
using CinemaDataService.Infrastructure.Repositories.UnitOfWork;

namespace CinemaDataService.Api.Queries.StudioQueries
{
    public class StudiosQueryHandler : IRequestHandler<StudiosQuery, IEnumerable<StudioResponse>>
    {
        IUnitOfWork _unitOfWork;
        IMapper _mapper;
        public StudiosQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            IUnitOfWork _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<StudioResponse>> Handle(StudiosQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<Studio>? Studios;
            if (request.Email != null)
                Studios = await _unitOfWork.Studios.GetByEmail(request.Email, cancellationToken);
            else Studios = await _unitOfWork.Studios.GetAll(null, cancellationToken, request.Pg);

            if (Studios == null)
                throw new TrialStudioNotFoundException("No Studio was found", request.Email);

            return _mapper.Map<IEnumerable<Studio>, IEnumerable<StudioResponse>>(Studios);
        }
    }
}
