using AutoMapper;
using MediatR;
using CinemaDataService.Domain.Aggregates.CinemaAggregate;
using CinemaDataService.Infrastructure.Models.CinemaDTO;
using CinemaDataService.Infrastructure.Repositories.Abstractions;
using CinemaDataService.Api.Exceptions.InfrastructureExceptions;

namespace CinemaDataService.Api.Commands.CinemaCommands.UpdateCommands
{
    public class UpdateCinemaCommandHandler : IRequestHandler<UpdateCinemaCommand, CinemaResponse>
    {
        ICinemaRepository _repository;
        IMapper _mapper;

        public UpdateCinemaCommandHandler(ICinemaRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<CinemaResponse> Handle(UpdateCinemaCommand request, CancellationToken cancellationToken)
        {
            Cinema cinema = _mapper.Map<UpdateCinemaCommand, Cinema>(request);

            Cinema? updated = await _repository.Update(cinema);

            if (updated == null)
            {
                throw new NotFoundException(cinema.Id, "Cinemas");
            }

            return _mapper.Map<Cinema, CinemaResponse>(updated);
        }
    }
}