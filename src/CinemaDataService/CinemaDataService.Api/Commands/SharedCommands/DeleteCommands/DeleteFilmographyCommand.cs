namespace CinemaDataService.Api.Commands.SharedCommands.DeleteCommands
{
    public class DeleteFilmographyCommand:DeleteRecordCommand
    {
        public DeleteFilmographyCommand(string? parentId, string cinemaId) : base(parentId, cinemaId) { }
    }
}
