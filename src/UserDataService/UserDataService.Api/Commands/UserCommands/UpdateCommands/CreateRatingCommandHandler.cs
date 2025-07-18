using AutoMapper;
using MediatR;
using UserDataService.Domain.Aggregates.UserAggregate;
using UserDataService.Infrastructure.Models.RatingDTO;
using UserDataService.Infrastructure.Repositories.Abstractions;
using UserDataService.Api.Exceptions.UserExceptions;
using MongoDB.Bson;

namespace UserDataService.Api.Commands.UserCommands.UpdateCommands
{
    public class UpdateRatingCommandHandler : IRequestHandler<UpdateRatingCommand, RatingResponse>
    {
        IUserRepository _repository;
        IMapper _mapper;

        public UpdateRatingCommandHandler(IUserRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<RatingResponse> Handle(UpdateRatingCommand request, CancellationToken cancellationToken)
        {
            RatingRecord rating = _mapper.Map<UpdateRatingCommand, RatingRecord>(request);

            rating.Id = ObjectId.GenerateNewId(DateTime.Now).ToString();

            RatingRecord? added = await _repository.UpdateRatingList(rating, cancellationToken);

            if (added is null)
            {
                throw new RecordAdditionFailedException("rating_records");
            }

            return _mapper.Map<RatingRecord, RatingResponse>(added);
        }
    }
}