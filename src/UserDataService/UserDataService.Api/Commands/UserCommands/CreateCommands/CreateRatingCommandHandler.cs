using AutoMapper;
using MediatR;
using UserDataService.Domain.Aggregates.UserAggregate;
using UserDataService.Infrastructure.Models.RatingDTO;
using UserDataService.Infrastructure.Repositories.Abstractions;
using UserDataService.Api.Exceptions.UserExceptions;

namespace UserDataService.Api.Commands.UserCommands.CreateCommands
{
    public class CreateRatingCommandHandler : IRequestHandler<CreateRatingCommand, RatingResponse>
    {
        IUserRepository _repository;
        IMapper _mapper;

        public CreateRatingCommandHandler(IUserRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<RatingResponse> Handle(CreateRatingCommand request, CancellationToken cancellationToken)
        {
            RatingRecord rating = _mapper.Map<CreateRatingCommand, RatingRecord>(request);
            RatingRecord? added = await _repository.AddToRatingList(rating, cancellationToken);

            if (added is null)
            {
                throw new RecordAdditionFailedException("rating_records");
            }

            return _mapper.Map<RatingRecord, RatingResponse>(added);
        }
    }
}