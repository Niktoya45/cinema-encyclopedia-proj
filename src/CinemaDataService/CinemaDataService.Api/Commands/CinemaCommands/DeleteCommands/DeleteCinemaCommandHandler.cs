using MediatR;
using CinemaDataService.Domain.Aggregates.CinemaAggregate;
using CinemaDataService.Infrastructure.Repositories.Abstractions;
using CinemaDataService.Api.Exceptions.InfrastructureExceptions;

namespace CinemaDataService.Api.Commands.CinemaCommands.DeleteCommands
{
    public class DeleteCinemaCommandHandler : IRequestHandler<DeleteCinemaCommand, Unit>
    {
        ICinemaRepository _repository;

        public DeleteCinemaCommandHandler(ICinemaRepository repository)
        {
            _repository = repository;
        }
        public async Task<Unit> Handle(DeleteCinemaCommand request, CancellationToken cancellationToken)
        {
            Cinema? deleted = await _repository.Delete(request.Id, cancellationToken);

            if (deleted == null)
            {
                throw new NotFoundException(deleted.Id, "Cinemas");
            }

            return Unit.Value;
        }
    }
}