using AutoMapper;
using CinemaDataService.Api.Commands.CinemaCommands.UpdateCommands;
using CinemaDataService.Api.Exceptions.InfrastructureExceptions;
using CinemaDataService.Domain.Aggregates.StudioAggregate;
using CinemaDataService.Infrastructure.Models.SharedDTO;
using CinemaDataService.Infrastructure.Repositories.Abstractions;
using MediatR;

namespace CinemaDataService.Api.Commands.StudioCommands.UpdateCommands
{
    public class UpdateStudioPictureCommandHandler : IRequestHandler<UpdateStudioPictureCommand, UpdatePictureResponse>
    {
        IStudioRepository _repository;
        IMapper _mapper;

        public UpdateStudioPictureCommandHandler(IStudioRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<UpdatePictureResponse> Handle(UpdateStudioPictureCommand request, CancellationToken cancellationToken)
        {
            //handle data validation
            Studio? updated = await _repository.UpdatePicture(request.Id, request.Picture, cancellationToken);

            if (updated == null)
            {
                throw new NotFoundException(request.Id, "Studios");
            }

            return new UpdatePictureResponse { Id = request.Id, Picture = request.Picture }; ;
        }
    }
}
