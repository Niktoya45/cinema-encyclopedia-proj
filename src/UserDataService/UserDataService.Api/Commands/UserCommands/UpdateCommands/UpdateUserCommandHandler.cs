using AutoMapper;
using MediatR;
using UserDataService.Domain.Aggregates.UserAggregate;
using UserDataService.Infrastructure.Models.UserDTO;
using UserDataService.Infrastructure.Repositories.Abstractions;
//using UserDataService.Api.Exceptions.UserExceptions;

namespace UserDataService.Api.Commands.UserCommands.UpdateCommands
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UpdateUserResponse>
    {
        IUserRepository _repository;
        IMapper _mapper;

        public UpdateUserCommandHandler(IUserRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<UpdateUserResponse> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            User User = _mapper.Map<UpdateUserCommand, User>(request);

            User? updated = await _repository.Update(User, cancellationToken);

            if (updated == null)
            {
                // handle;
            }

            return _mapper.Map<User, UpdateUserResponse>(updated);
        }
    }
}