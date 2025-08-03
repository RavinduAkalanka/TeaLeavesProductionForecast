using TeaLeavesProductionForecastWebAPI.Enums;

namespace TeaLeavesProductionForecastWebAPI.DTO
{
    public class RegisterUserDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string Area { get; set; }
        public string Estate { get; set; }
        public Role Role { get; set; }
    }

    public class UserResponseDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Area { get; set; }
        public string Estate { get; set; }
        public string Role { get; set; }
    }
}
