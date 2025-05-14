namespace Shared.CinemaDataService.Models.RecordDTO
{
    public class UpdateProductionStudioRequest
    {
        public string Name { get; set; }
        public string? Picture { get; set; } = default;
    }
}
