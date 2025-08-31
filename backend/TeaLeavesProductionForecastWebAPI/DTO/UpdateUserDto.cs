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

    public class ResetPasswordRequest
    {
        public int UserId { get; set; }
    }

    public class ResetPasswordOtpRequest
    {
        public int UserId { get; set; }
        public string Otp { get; set; } 
        public string NewPassword { get; set; } 
    }

    public class ResetPasswordRequestWithEmail
    {
        public string Email { get; set; }
    }

    public class ResetPasswordOtpRequestWithEmail
    {
        public string Email { get; set; }
        public string Otp { get; set; }
        public string NewPassword { get; set; }
    }

}
