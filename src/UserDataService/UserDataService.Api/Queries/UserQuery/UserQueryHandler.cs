using AutoMapper;
using MediatR;
using UserDataService.Infrastructure.Models.UserDTO;
using UserDataService.Domain.Aggregates.UserAggregate;
//using UserDataService.Api.Exceptions.UserExceptions;
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
            User? User = await _repository.FindById(request.Id, cancellationToken);

            if (User == null) { 
                // handle
            }


            return _mapper.Map<GetUserResponse>(User);
        }
    }
}
