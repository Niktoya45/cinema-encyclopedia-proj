using AutoMapper;
using MediatR;
using CinemaDataService.Infrastructure.Models.CinemaDTO;
using CinemaDataService.Domain.Aggregates.CinemaAggregate;
using CinemaDataService.Infrastructure.Repositories.Abstractions;
using CinemaDataService.Api.Exceptions.InfrastructureExceptions;

namespace CinemaDataService.Api.Queries.CinemaQueries
{

    public class CinemaQueryHandler : IRequestHandler<CinemaQuery, CinemaResponse>
    {
        ICinemaRepository _repository;
        IMapper _mapper;
        public CinemaQueryHandler(ICinemaRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<CinemaResponse> Handle(CinemaQuery request, CancellationToken cancellationToken)
        {
            Cinema? cinema = await _repository.FindById(request.Id, cancellationToken);

            if (cinema == null) {
                // handle
                throw new NotFoundException(request.Id, "Cinemas");
            }


            return _mapper.Map<CinemaResponse>(cinema);
        }
    }
}
