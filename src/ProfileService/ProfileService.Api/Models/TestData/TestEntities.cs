using ProfileService.Api.Models.Display;

namespace ProfileService.Api.Models.TestData
{
    public static class TestEntities
    {
        public const bool Used = false;

        public static Profile Profile = new Profile
        {
            Id = "2",
            Username = "Long Long Profile Name",
            Birthdate = new DateOnly(2000, 1, 1),
            //Picture = "/img/logo_placeholder.png",
            Description = "Generally interested in alternative horrors and.."
        };

        public static string Role = "User,Administrator";

    }
}
