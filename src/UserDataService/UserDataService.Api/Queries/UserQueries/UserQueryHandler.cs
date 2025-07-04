using AutoMapper;
using MediatR;
using UserDataService.Infrastructure.Models.UserDTO;
using UserDataService.Domain.Aggregates.UserAggregate;
using UserDataService.Api.Exceptions.InfrastructureExceptions;
using UserDataService.Infrastructure.Repositories.Abstractions;

namespace UserDataService.Api.Queries.UserQueries
{

    public class UserQueryHandler : IRequestHandler<UserQuery, UserResponse>
    {
        IUserRepository _repository;
        IMapper _mapper;
        public UserQueryHandler(IUserRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<UserResponse> Handle(UserQuery request, CancellationToken cancellationToken)
        {
            User? user = await _repository.FindById(request.Id, cancellationToken);

            if (user == null) {
                throw new NotFoundException(request.Id, "Users");
            }


            return _mapper.Map<User, UserResponse>(user);
        }
    }
}
