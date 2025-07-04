
namespace UserDataService.Infrastructure.Models.UserDTO
{
    public class CreateUserRequest {

        public string Username { get; set; }
        public DateOnly? Birthdate { get; set; } = default(DateOnly);
        public string? Picture { get; set; }
        public string? Description { get; set; }
    }
}