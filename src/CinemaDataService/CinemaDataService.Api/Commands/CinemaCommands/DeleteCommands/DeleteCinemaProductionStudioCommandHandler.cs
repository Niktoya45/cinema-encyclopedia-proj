using CinemaDataService.Api.Exceptions.InfrastructureExceptions;
using CinemaDataService.Domain.Aggregates.CinemaAggregate;
using CinemaDataService.Infrastructure.Repositories.Abstractions;
using MediatR;

namespace CinemaDataService.Api.Commands.CinemaCommands.DeleteCommands
{
    public class DeleteCinemaProductionStudioCommandHandler : IRequestHandler<DeleteCinemaProductionStudioCommand, Unit>
    {
        ICinemaRepository _repository;

        public DeleteCinemaProductionStudioCommandHandler(ICinemaRepository repository)
        {
            _repository = repository;
        }
        public async Task<Unit> Handle(DeleteCinemaProductionStudioCommand request, CancellationToken cancellationToken)
        {
            StudioRecord? deleted = await _repository.DeleteProductionStudio(request.ParentId, new StudioRecord {Id=request.Id}, cancellationToken);

            if (deleted == null)
            {
                throw new NotFoundException(request.Id, "Cinemas");
            }

            return Unit.Value;
        }
    }
}
