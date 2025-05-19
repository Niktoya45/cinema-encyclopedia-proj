using AutoMapper;
using MediatR;
using UserDataService.Domain.Aggregates.UserAggregate;
using UserDataService.Infrastructure.Models.RatingDTO;
using UserDataService.Infrastructure.Repositories.Abstractions;

namespace UserDataService.Api.Queries.UserQueries
{
    public class UserCinemaRatingQueryHandler : IRequestHandler<UserCinemaRatingQuery, RatingResponse?>
    {
        IUserRepository _repository;
        IMapper _mapper;
        public UserCinemaRatingQueryHandler(IUserRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<RatingResponse?> Handle(UserCinemaRatingQuery request, CancellationToken cancellationToken)
        {
            RatingRecord? rating = await _repository.FindRatingByUserIdCinemaId(request.UserId, request.CinemaId, cancellationToken);

            return rating == null ? null : _mapper.Map<RatingRecord, RatingResponse>(rating);
        }
    }
}
