using UserDataService.Api.Exceptions.InfrastructureExceptions;
using UserDataService.Domain.Aggregates.UserAggregate;
using UserDataService.Infrastructure.Models.SharedDTO;
using UserDataService.Infrastructure.Repositories.Abstractions;
using UserDataService.Api.Commands.UserCommands.UpdateCommands;
using MediatR;

namespace UserDataService.Api.Commands.CinemaCommands.UpdateCommands
{
    public class UpdateUserPictureCommandHandler : IRequestHandler<UpdateUserPictureCommand, UpdatePictureResponse>
    {
        IUserRepository _repository;

        public UpdateUserPictureCommandHandler(IUserRepository repository)
        {
            _repository = repository;
        }
        public async Task<UpdatePictureResponse> Handle(UpdateUserPictureCommand request, CancellationToken cancellationToken)
        {
            //handle data validation
            User? updated = await _repository.UpdatePicture(request.Id, request.Picture, cancellationToken);

            if (updated == null)
            {
                throw new NotFoundException(request.Id, "Users");
            }

            return new UpdatePictureResponse { Id = request.Id, Picture = request.Picture };
        }
    }
}
