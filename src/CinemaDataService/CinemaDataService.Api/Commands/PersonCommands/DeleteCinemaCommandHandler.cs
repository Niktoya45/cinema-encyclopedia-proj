using MediatR;
using CinemaDataService.Domain.Aggregates.PersonAggregate;
using CinemaDataService.Infrastructure.Repositories.Abstractions;
using CinemaDataService.Api.Exceptions.PersonExceptions;

namespace CinemaDataService.Api.Commands.PersonCommands
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
                // handle
            }

            return Unit.Value;
        }
    }
}