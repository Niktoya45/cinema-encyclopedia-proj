using AutoMapper;
using MediatR;
using CinemaDataService.Infrastructure.Models.CinemaDTO;
using CinemaDataService.Domain.Aggregates.CinemaAggregate;
using CinemaDataService.Api.Exceptions.CinemaExceptions;
using CinemaDataService.Infrastructure.Repositories.UnitOfWork;

namespace CinemaDataService.Api.Queries.CinemaQueries
{

    public class CinemaQueryHandler : IRequestHandler<CinemaQuery, CinemaResponse>
    {
        IUnitOfWork _unitOfWork;
        IMapper _mapper;
        public CinemaQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<CinemaResponse> Handle(CinemaQuery request, CancellationToken cancellationToken)
        {
            Cinema? Cinema = await _unitOfWork.Cinemas.GetById(request.Id, cancellationToken);

            if (Cinema == null)
                throw new TrialCinemaNotFoundException(request.Id);


            return _mapper.Map<CinemaResponse>(Cinema);
        }
    }
}
