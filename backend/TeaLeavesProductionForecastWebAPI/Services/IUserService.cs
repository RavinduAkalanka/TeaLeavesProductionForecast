using TeaLeavesProductionForecastWebAPI.DTO;

namespace TeaLeavesProductionForecastWebAPI.Services
{
    public interface IUserService
    {
        Task<UserResponseDto> RegisterUserAsync(RegisterUserDto dto);
        Task<string?> LoginUserAsync(LoginRequestDto dto);
    }
}
