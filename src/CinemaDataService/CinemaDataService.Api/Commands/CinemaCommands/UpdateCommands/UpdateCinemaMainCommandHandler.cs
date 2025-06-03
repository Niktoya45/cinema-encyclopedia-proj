using AutoMapper;
using CinemaDataService.Api.Exceptions.InfrastructureExceptions;
using CinemaDataService.Domain.Aggregates.CinemaAggregate;
using CinemaDataService.Infrastructure.Models.CinemaDTO;
using CinemaDataService.Infrastructure.Repositories.Abstractions;
using MediatR;

namespace CinemaDataService.Api.Commands.CinemaCommands.UpdateCommands
{
    public class UpdateCinemaMainCommandHandler : IRequestHandler<UpdateCinemaMainCommand, CinemaResponse>
    {
        ICinemaRepository _repository;
        IMapper _mapper;

        public UpdateCinemaMainCommandHandler(ICinemaRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<CinemaResponse> Handle(UpdateCinemaMainCommand request, CancellationToken cancellationToken)
        {
            Cinema cinema = _mapper.Map<UpdateCinemaMainCommand, Cinema>(request);

            Cinema? updated = await _repository.UpdateMain(cinema);

            if (updated == null)
            {
                throw new NotFoundException(cinema.Id, "Cinemas");
            }

            return _mapper.Map<Cinema, CinemaResponse>(updated);
        }
    }
}
