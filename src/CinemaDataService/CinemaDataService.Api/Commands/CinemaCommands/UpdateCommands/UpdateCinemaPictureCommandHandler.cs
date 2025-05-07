using AutoMapper;
using CinemaDataService.Api.Exceptions.InfrastructureExceptions;
using CinemaDataService.Domain.Aggregates.CinemaAggregate;
using CinemaDataService.Infrastructure.Models.SharedDTO;
using CinemaDataService.Infrastructure.Repositories.Abstractions;
using MediatR;

namespace CinemaDataService.Api.Commands.CinemaCommands.UpdateCommands
{
    public class UpdateCinemaPictureCommandHandler : IRequestHandler<UpdateCinemaPictureCommand, UpdatePictureResponse>
    {
        ICinemaRepository _repository;

        public UpdateCinemaPictureCommandHandler(ICinemaRepository repository)
        {
            _repository = repository;
        }
        public async Task<UpdatePictureResponse> Handle(UpdateCinemaPictureCommand request, CancellationToken cancellationToken)
        {
            //handle data validation
            Cinema? updated = await _repository.UpdatePicture(request.Id, request.Picture, cancellationToken);

            if (updated == null)
            {
                throw new NotFoundException(request.Id, "Cinemas");
            }

            return new UpdatePictureResponse { Id = request.Id, Picture = request.Picture };
        }
    }
}
