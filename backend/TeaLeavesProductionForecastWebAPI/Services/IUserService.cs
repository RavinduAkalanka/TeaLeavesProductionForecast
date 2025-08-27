using TeaLeavesProductionForecastWebAPI.DTO;

namespace TeaLeavesProductionForecastWebAPI.Services
{
    public interface IUserService
    {
        Task<UserResponseDto> RegisterUserAsync(RegisterUserDto dto);
        Task<string?> LoginUserAsync(LoginRequestDto dto);
        Task<UserResponseDto?> GetUserByIdAsync(int userId);
        Task<string?> UpdateUserAsync(int id, UpdateUserDto dto);
    }
}
