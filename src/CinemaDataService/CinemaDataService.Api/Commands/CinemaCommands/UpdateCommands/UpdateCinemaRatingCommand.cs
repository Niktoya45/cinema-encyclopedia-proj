using CinemaDataService.Domain.Aggregates.CinemaAggregate;
using CinemaDataService.Infrastructure.Models.SharedDTO;
using MediatR;

namespace CinemaDataService.Api.Commands.CinemaCommands.UpdateCommands
{
    public class UpdateCinemaRatingCommand : IRequest<UpdateRatingResponse>
    {
        public string Id { get; set; }
        public double Rating { get; set; }

        public UpdateCinemaRatingCommand(string id, double rating)
        {
            Id = id;
            Rating = rating;
        }
    }
}
