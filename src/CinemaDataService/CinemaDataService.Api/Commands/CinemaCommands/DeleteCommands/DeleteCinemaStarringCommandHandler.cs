using CinemaDataService.Api.Exceptions.InfrastructureExceptions;
using CinemaDataService.Domain.Aggregates.CinemaAggregate;
using CinemaDataService.Infrastructure.Repositories.Abstractions;
using MediatR;

namespace CinemaDataService.Api.Commands.CinemaCommands.DeleteCommands
{
    public class DeleteCinemaStarringCommandHandler : IRequestHandler<DeleteCinemaStarringCommand, Unit>
    {
        ICinemaRepository _repository;

        public DeleteCinemaStarringCommandHandler(ICinemaRepository repository)
        {
            _repository = repository;
        }
        public async Task<Unit> Handle(DeleteCinemaStarringCommand request, CancellationToken cancellationToken)
        {
            Starring? deleted = await _repository.DeleteStarring(request.ParentId, new Starring { Id = request.Id }, cancellationToken);

            if (deleted == null)
            {
                throw new NotFoundException(request.Id, "Cinemas");
            }

            return Unit.Value;
        }
    }
}
