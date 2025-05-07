using Microsoft.AspNetCore.Mvc;

namespace EncyclopediaService.Api.Models.Sort
{
    public class SortEntity
    {
        public Dictionary<string, string> MapToDisplay { get; set; } = default!;

        public Dictionary<string, string> MapToQueryKey { get; set; } = default!;
    }
}
