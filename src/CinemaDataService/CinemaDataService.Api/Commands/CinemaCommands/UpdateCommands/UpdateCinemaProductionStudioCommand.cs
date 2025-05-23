using CinemaDataService.Api.Commands.SharedCommands.UpdateCommands;


namespace CinemaDataService.Api.Commands.CinemaCommands.UpdateCommands
{
    public class UpdateCinemaProductionStudioCommand:UpdateProductionStudioCommand
    {
        public UpdateCinemaProductionStudioCommand(
            string? cinemaId,
            string studioId,
            string name,
            string? picture
        ):base(studioId, name, picture)
        {
            CinemaId = cinemaId;
        }
        
        public string? CinemaId { get; set; }
    }

}
