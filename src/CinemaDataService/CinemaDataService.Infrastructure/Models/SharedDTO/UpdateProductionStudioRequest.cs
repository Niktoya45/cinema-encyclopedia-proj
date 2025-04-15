
namespace CinemaDataService.Infrastructure.Models.SharedDTO
{
    public class UpdateProductionStudioRequest
    {
        public string Name { get; set; }
        public string? Picture { get; set; } = default(string);
    }
}
