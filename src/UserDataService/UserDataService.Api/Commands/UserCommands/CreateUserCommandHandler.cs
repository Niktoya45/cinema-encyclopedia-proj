using AutoMapper;
using MediatR;
using UserDataService.Domain.Aggregates.UserAggregate;
using UserDataService.Infrastructure.Models.UserDTO;
using UserDataService.Infrastructure.Repositories;

namespace UserDataService.Api.Commands.UserCommands
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CreateUserResponse>
    {
        IUnitOfWork _unitOfWork;
        IMapper _mapper;

        public CreateUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<CreateUserResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            User User = _mapper.Map<CreateUserCommand, User>(request);
            User added = _unitOfWork.Cities.Add(User);

            return _mapper.Map<User, CreateUserResponse>(added);
        }
    }
}