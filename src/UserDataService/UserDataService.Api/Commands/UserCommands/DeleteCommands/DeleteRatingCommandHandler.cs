using AutoMapper;
using MediatR;
using UserDataService.Api.Commands.UserCommands.CreateCommands;
using UserDataService.Api.Exceptions.InfrastructureExceptions;
using UserDataService.Domain.Aggregates.UserAggregate;
using UserDataService.Infrastructure.Models.RatingDTO;
using UserDataService.Infrastructure.Repositories.Abstractions;

namespace UserDataService.Api.Commands.UserCommands.DeleteCommands
{
    public class DeleteRatingCommandHandler : IRequestHandler<DeleteRatingCommand, RatingResponse>
    {
        IUserRepository _repository;
        IMapper _mapper;

        public DeleteRatingCommandHandler(IUserRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<RatingResponse> Handle(DeleteRatingCommand request, CancellationToken cancellationToken)
        {
            RatingRecord rating = _mapper.Map<DeleteRatingCommand, RatingRecord>(request);
            RatingRecord? added = await _repository.AddToRatingList(rating, cancellationToken);

            if (added is null)
            {
                throw new RecordNotFoundException(request.UserId, request.CinemaId, "rating_records");
            }

            return _mapper.Map<RatingRecord, RatingResponse>(added);
        }
    }
}