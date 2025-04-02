using MediatR;
using CinemaDataService.Domain.Aggregates.StudioAggregate;
using CinemaDataService.Infrastructure.Repositories.Abstractions;
using CinemaDataService.Api.Exceptions.StudioExceptions;

namespace CinemaDataService.Api.Commands.StudioCommands
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
                // handle;
            }

            return Unit.Value;
        }
    }
}