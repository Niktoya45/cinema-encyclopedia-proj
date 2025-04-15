using CinemaDataService.Api.Commands.SharedCommands.CreateCommands;

namespace CinemaDataService.Api.Commands.CinemaCommands.CreateCommands
{
    public class CreateCinemaProductionStudioCommand:CreateProductionStudioCommand
    {
        public CreateCinemaProductionStudioCommand(
            string cinemaId,
            string id,
            string name,
            string? picture
        ) : base(id, name, picture) 
        {
            CinemaId = cinemaId;
        }

        public string CinemaId { get; }
    }
}
