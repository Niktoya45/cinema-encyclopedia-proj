using AutoMapper;
using MediatR;
using CinemaDataService.Api.Queries;
using CinemaDataService.Domain.Aggregates.CinemaAggregate;
using CinemaDataService.Api.Exceptions.CinemaExceptions;
using CinemaDataService.Infrastructure.Repositories.UnitOfWork;

namespace CinemaDataService.Api.Queries.CinemaQueries
{
    public class CinemasQueryHandler : IRequestHandler<CinemasQuery, IEnumerable<GetCinemasResponse>>
    {
        IUnitOfWork _unitOfWork;
        IMapper _mapper;
        public CinemasQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            IUnitOfWork _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetCinemasResponse>> Handle(CinemasQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<Cinema>? Cinemas;
            if (request.Email != null)
                Cinemas = await _unitOfWork.Cinemas.GetByEmail(request.Email, cancellationToken);
            else Cinemas = await _unitOfWork.Cinemas.GetAll(null, cancellationToken, request.Pg);

            if (Cinemas == null)
                throw new TrialCinemaNotFoundException("No Cinema was found", request.Email);

            return _mapper.Map<IEnumerable<Cinema>, IEnumerable<GetCinemasResponse>>(Cinemas);
        }
    }
}
