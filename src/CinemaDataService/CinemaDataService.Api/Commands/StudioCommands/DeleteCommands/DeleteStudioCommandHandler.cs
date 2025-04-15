using MediatR;
using CinemaDataService.Domain.Aggregates.StudioAggregate;
using CinemaDataService.Infrastructure.Repositories.Abstractions;
using CinemaDataService.Api.Exceptions.InfrastructureExceptions;

namespace CinemaDataService.Api.Commands.StudioCommands.DeleteCommands
{
    public class DeleteStudioCommandHandler : IRequestHandler<DeleteStudioCommand, Unit>
    {
        IStudioRepository _repository;

        public DeleteStudioCommandHandler(IStudioRepository repository)
        {
            _repository = repository;
        }
        public async Task<Unit> Handle(DeleteStudioCommand request, CancellationToken cancellationToken)
        {
            Studio? deleted = await _repository.Delete(request.Id, cancellationToken);

            if (deleted == null)
            {
                throw new NotFoundException(request.Id, "Studios");
            }

            return Unit.Value;
        }
    }
}