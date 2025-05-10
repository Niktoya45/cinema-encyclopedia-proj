using AutoMapper;
using MediatR;
using UserDataService.Infrastructure.Models.UserDTO;
using UserDataService.Domain.Aggregates.UserAggregate;
using UserDataService.Api.Exceptions.InfrastructureExceptions;
using UserDataService.Infrastructure.Repositories.Abstractions;

namespace UserDataService.Api.Queries.UserQueries
{

    public class UserQueryHandler : IRequestHandler<UserQuery, GetUserResponse>
    {
        IUserRepository _repository;
        IMapper _mapper;
        public UserQueryHandler(IUserRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<GetUserResponse> Handle(UserQuery request, CancellationToken cancellationToken)
        {
            User? user = await _repository.FindById(request.Id, cancellationToken);

            if (user == null) {
                new NotFoundException(request.Id, "Users");
            }


            return _mapper.Map<GetUserResponse>(user);
        }
    }
}
