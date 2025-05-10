using CinemaDataService.Api.Exceptions.InfrastructureExceptions;
using CinemaDataService.Domain.Aggregates.CinemaAggregate;
using CinemaDataService.Infrastructure.Models.SharedDTO;
using CinemaDataService.Infrastructure.Repositories.Abstractions;
using MediatR;

namespace CinemaDataService.Api.Commands.CinemaCommands.UpdateCommands
{
    public class UpdateCinemaRatingCommandHandler : IRequestHandler<UpdateCinemaRatingCommand, UpdateRatingResponse>
    {
        ICinemaRepository _repository;

        public UpdateCinemaRatingCommandHandler(ICinemaRepository repository)
        {
            _repository = repository;
        }
        public async Task<UpdateRatingResponse> Handle(UpdateCinemaRatingCommand request, CancellationToken cancellationToken)
        {
            //handle data validation
            Cinema? updated = await _repository.UpdateRating(request.Id, request.Rating, cancellationToken);

            if (updated == null)
            {
                throw new NotFoundException(request.Id, "Cinemas");
            }

            return new UpdateRatingResponse { Id = request.Id, Name=updated.Name, Rating = updated.Rating };
        }
    }
}
