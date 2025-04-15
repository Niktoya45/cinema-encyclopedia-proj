using MediatR;
using CinemaDataService.Domain.Aggregates.PersonAggregate;
using CinemaDataService.Infrastructure.Repositories.Abstractions;
using CinemaDataService.Api.Exceptions.InfrastructureExceptions;

namespace CinemaDataService.Api.Commands.PersonCommands.DeleteCommands
{
    public class DeletePersonCommandHandler : IRequestHandler<DeletePersonCommand, Unit>
    {
        IPersonRepository _repository;

        public DeletePersonCommandHandler(IPersonRepository repository)
        {
            _repository = repository;
        }
        public async Task<Unit> Handle(DeletePersonCommand request, CancellationToken cancellationToken)
        {
            Person? deleted = await _repository.Delete(request.Id, cancellationToken);

            if (deleted == null)
            {
                throw new NotFoundException(request.Id, "Persons");
            }

            return Unit.Value;
        }
    }
}