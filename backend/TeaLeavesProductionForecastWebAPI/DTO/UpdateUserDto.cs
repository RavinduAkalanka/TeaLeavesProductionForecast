using TeaLeavesProductionForecastWebAPI.Enums;

namespace TeaLeavesProductionForecastWebAPI.DTO
{
    public class UpdateUserDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Area { get; set; }
        public string Estate { get; set; }
        public Role Role { get; set; }
    }
}
