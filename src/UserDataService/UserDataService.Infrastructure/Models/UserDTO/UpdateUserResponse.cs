
namespace UserDataService.Infrastructure.Models.UserDTO
{
    public class UpdateUserResponse
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public DateOnly Birthdate { get; set; }
        public string Picture { get; set; }
        public string PictureUri { get; set; }
        public string Description { get; set; }
    }
}