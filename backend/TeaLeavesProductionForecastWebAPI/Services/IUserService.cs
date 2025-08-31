using TeaLeavesProductionForecastWebAPI.DTO;

namespace TeaLeavesProductionForecastWebAPI.Services
{
    public interface IUserService
    {
        Task<UserResponseDto> RegisterUserAsync(RegisterUserDto dto);
        Task<string?> LoginUserAsync(LoginRequestDto dto);
        Task<UserResponseDto?> GetUserByIdAsync(int userId);
        Task<string?> UpdateUserAsync(int id, UpdateUserDto dto);
        Task<bool> SendResetPasswordEmailAsync(int userId);
        Task<bool> VerifyOtpAndResetPasswordAsync(int userId, string otp, string newPassword);
        Task<bool> SendResetPasswordEmailByEmailAsync(string email);
        Task<bool> VerifyOtpAndResetPasswordByEmailAsync(string email, string otp, string newPassword);
    }
}
