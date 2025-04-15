using AutoMapper;
using MediatR;
using UserDataService.Domain.Aggregates.UserAggregate;
using UserDataService.Infrastructure.Models.UserDTO;
using UserDataService.Infrastructure.Repositories.Abstractions;

namespace UserDataService.Api.Commands.UserCommands
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CreateUserResponse>
    {
        IUserRepository _repository;
        IMapper _mapper;

        public CreateUserCommandHandler(IUserRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<CreateUserResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            User User = _mapper.Map<CreateUserCommand, User>(request);
            User added = _repository.Add(User);

            return _mapper.Map<User, CreateUserResponse>(added);
        }
    }
}