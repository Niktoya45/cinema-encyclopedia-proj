using AutoMapper;
using MediatR;
using UserDataService.Infrastructure.Models.UserDTO;
using UserDataService.Domain.Aggregates.UserAggregate;
using UserDataService.Api.Exceptions.UserExceptions;
using UserDataService.Infrastructure.Repositories.UnitOfWork;

namespace UserDataService.Api.Queries.UserQueries
{

    public class UserQueryHandler : IRequestHandler<UserQuery, GetUserResponse>
    {
        IUnitOfWork _unitOfWork;
        IMapper _mapper;
        public UserQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<GetUserResponse> Handle(UserQuery request, CancellationToken cancellationToken)
        {
            User? User = await _unitOfWork.Users.GetById(request.Id, cancellationToken);

            if (User == null)
                throw new TrialUserNotFoundException(request.Id);


            return _mapper.Map<GetUserResponse>(User);
        }
    }
}
