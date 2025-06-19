using AutoMapper;
using MediatR;
using UserDataService.Infrastructure.Models.LabeledDTO;
using UserDataService.Domain.Aggregates.UserAggregate;
using UserDataService.Api.Exceptions.InfrastructureExceptions;
using UserDataService.Infrastructure.Repositories.Abstractions;
using UserDataService.Infrastructure.Models.SharedDTO;

namespace UserDataService.Api.Queries.UserQueries
{

    public class UserLabeledQueryHandler : IRequestHandler<UserLabeledQuery, IEnumerable<LabeledResponse>>
    {
        IUserRepository _repository;
        IMapper _mapper;
        public UserLabeledQueryHandler(IUserRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<LabeledResponse>> Handle(UserLabeledQuery request, CancellationToken cancellationToken)
        {
            IList<LabeledRecord>? labeled = await _repository.FindCinemasByUserIdLabel(request.Id, request.Label, request.CinemaId, null, cancellationToken);

            if (labeled == null) {
                throw new NotFoundException(request.Id, "Users");
            }

            return _mapper.Map<IEnumerable<LabeledRecord>, IEnumerable<LabeledResponse>>(labeled);
        }
    }
}
