using System.Data;
using TeaLeavesProductionForecastWebAPI.Enums;

namespace TeaLeavesProductionForecastWebAPI.Model
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string PasswordHash { get; set; }
        public string Area { get; set; }
        public string Estate { get; set; }
        public Role Role { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
