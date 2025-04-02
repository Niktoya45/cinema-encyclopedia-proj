using AutoMapper;
using MediatR;
using UserDataService.Domain.Aggregates.UserAggregate;
using UserDataService.Infrastructure.Models.UserDTO;
using UserDataService.Infrastructure.Repositories.UnitOfWork;
//using UserDataService.Api.Exceptions.UserExceptions;

namespace UserDataService.Api.Commands.UserCommands
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UpdateUserResponse>
    {
        IUnitOfWork _unitOfWork;
        IMapper _mapper;

        public UpdateUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<UpdateUserResponse> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            User User = _mapper.Map<UpdateUserCommand, User>(request);

            User? updated = await _unitOfWork.Cities.Update(User);

            if (updated == null)
            {
                // handle;
            }

            return _mapper.Map<User, UpdateUserResponse>(updated);
        }
    }
}