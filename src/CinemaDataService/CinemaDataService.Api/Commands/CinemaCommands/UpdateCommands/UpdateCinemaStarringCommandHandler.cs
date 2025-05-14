using AutoMapper;
using CinemaDataService.Api.Commands.CinemaCommands.UpdateCommands;
using CinemaDataService.Api.Exceptions.InfrastructureExceptions;
using CinemaDataService.Domain.Aggregates.CinemaAggregate;
using CinemaDataService.Infrastructure.Models.RecordDTO;
using CinemaDataService.Infrastructure.Repositories.Abstractions;
using MediatR;

namespace CinemaDataService.Api.Commands.CinemaCommands.UpdateCommands
{
    public class UpdateCinemaStarringCommandHandler : IRequestHandler<UpdateCinemaStarringCommand, StarringResponse>
    {
        ICinemaRepository _repository;
        IMapper _mapper;

        public UpdateCinemaStarringCommandHandler(ICinemaRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<StarringResponse> Handle(UpdateCinemaStarringCommand request, CancellationToken cancellationToken)
        {
            //handle data validation
            Starring starring = _mapper.Map<UpdateCinemaStarringCommand, Starring>(request);

            Starring? updated = await _repository.UpdateStarring(starring, cancellationToken);

            if (updated == null)
            {
                throw new NotFoundException(request.Id, "Cinemas");
            }

            return _mapper.Map<Starring, StarringResponse>(updated);
        }
    }
}
