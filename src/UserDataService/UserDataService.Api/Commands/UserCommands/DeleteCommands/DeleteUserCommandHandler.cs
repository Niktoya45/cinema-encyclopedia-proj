using MediatR;
using UserDataService.Domain.Aggregates.UserAggregate;
using UserDataService.Infrastructure.Repositories.Abstractions;
//using UserDataService.Api.Exceptions;

namespace UserDataService.Api.Commands.UserCommands.DeleteCommands
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Unit>
    {
        IUserRepository _repository;

        public DeleteUserCommandHandler(IUserRepository repository)
        {
            _repository = repository;
        }
        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            User? deleted = await _repository.Delete(request.Id, cancellationToken);

            if (deleted == null)
            {
                // handle;
            }

            return Unit.Value;
        }
    }
}