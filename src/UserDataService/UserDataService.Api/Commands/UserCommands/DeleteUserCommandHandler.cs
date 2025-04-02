using MediatR;
using UserDataService.Domain.Aggregates.UserAggregate;
using UserDataService.Infrastructure.Repositories.UnitOfWork;
//using UserDataService.Api.Exceptions;

namespace UserDataService.Api.Commands.UserCommands
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Unit>
    {
        IUnitOfWork _unitOfWork;

        public DeleteUserCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            User? deleted = await _unitOfWork.Cities.Delete(request.Id, cancellationToken);

            if (deleted == null)
            {
                // handle;
            }

            return Unit.Value;
        }
    }
}