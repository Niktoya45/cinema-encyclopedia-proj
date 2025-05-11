
namespace UserDataService.Infrastructure.Models.UserDTO
{
    public class UpdateUserRequest
    {
        public string Username { get; set; }
        public DateOnly? Birthdate { get; set; }
        public string? Picture { get; set; }
        public string? Description { get; set; }
    }
}