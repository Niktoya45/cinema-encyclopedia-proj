

namespace Shared.CinemaDataService.Models.RecordDTO;

public record RatingScore
{
    public double Score { get; set; } = 0.0;
    public uint N { get; set; } = 0;
}
