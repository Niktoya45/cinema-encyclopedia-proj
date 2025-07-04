using Shared.UserDataService.Models.Flags;
using ProfileService.Api.Models.Display;

namespace ProfileService.Api.Models.TestData
{
    public static class TestRecords
    {
        public const bool Used = false;

        public static Label[] labels = Enum.GetValues<Label>();

        public static IList<Marked> Markeds = Enumerable.Range(1, 55).Select(x => new Marked
        {
            ParentId = "2",
            Id = "" + x,
            Name = "Cinema " + x,
            Label = labels[x%4 + 1],
            AddedAt = new DateTime(2020 + x / 10, x % 12 + 1, x % 30 + 1, x % 12 + 1, x % 60 + 1, x % 60 + 1),
            Picture = null
        }).ToList();
    }
}
